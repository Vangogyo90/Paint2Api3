using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paint2API.Models;
using System.Data;

namespace Paint2API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : Controller
    {
        private readonly PaintContext _context;

        public PriceController(PaintContext context)
        {
            _context = context;
        }

        [HttpGet("CalculateTotalPriceForDelivery/{deliveryID}")]
        public IActionResult CalculateTotalPriceForDelivery(int deliveryID)
        {
            IQueryable<ColorDelivery> data = _context.ColorDeliveries
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog);


            if (deliveryID > 0)
            {
                data = data.Where(delivery => delivery.DeliveryId == deliveryID);
            }

            var result = data.ToList()
    .GroupBy(a => a.ColorId)
    .Select(g => new ColorDelivery
    {
        ColorId = g.Key,
        Quantity = g.Count(),
        Colors = g.First().Colors,
        IdColorDelivery = g.First().IdColorDelivery,
        DeliveryId = g.First().DeliveryId,
        Deliverys = g.First().Deliverys
    });

            decimal totalPriceForDelivery = CalculateTotalPrice(result);

            var delivery = _context.Deliveries.FirstOrDefault(d => d.IdDelivery == deliveryID);

            if (delivery != null)
            {
                return Ok(totalPriceForDelivery);
            }
            else
            {
                return NotFound("Доставка не найдена.");
            }
        }

        private decimal CalculateTotalPrice(IEnumerable<ColorDelivery> colorDeliveries)
        {
            decimal totalPrice = 0;

            foreach (var colorDelivery in colorDeliveries)
            {
                if (colorDelivery.Quantity.HasValue)
                {
                    int quantity = colorDelivery.Quantity.Value;
                    double pricePerUnit = GetPricePerUnit(colorDelivery.ColorId);
                    decimal priceForItem = (decimal)(quantity * pricePerUnit);

                    totalPrice += priceForItem;
                }
            }

            return totalPrice;
        }

        private double GetPricePerUnit(int? colorId)
        {
            var quantityColor = _context.QuantityColors.FirstOrDefault(qc => qc.ColorId == colorId);
            if (quantityColor != null)
            {
                return quantityColor.Price_For_KG;
            }
            else
            {
                return 0;
            }
        }
    }
}
