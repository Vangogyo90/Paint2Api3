using System;
using System.Collections.Generic;

namespace Paint2API.Models
{
    public partial class QuantityColor
    {
        public int? IdQuantityColors { get; set; }
        public int? ColorId { get; set; }
        public int? WareHouseId { get; set; }
        public int Quantity { get; set; }
        public double Price_For_KG { get; set; }
        public DateTime? Date { get; set; }
        public int? Shelf_Life { get; set; }
        public Color? Colors { get; set; }
        public WareHouse? WareHouses { get; set; }
    }
}
