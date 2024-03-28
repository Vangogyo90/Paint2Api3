using System.ComponentModel.DataAnnotations.Schema;

namespace Paint2API.Models
{
    public class SoldColor
    {
        public int? ID_Sold_Color { get; set; }
        public int? Delivery_ID { get; set; }
        public double Price_Delivery { get; set; }
        public DateTime? Date { get; set; }
        [NotMapped]
        public Delivery? Delivery { get; set; }
    }
}
