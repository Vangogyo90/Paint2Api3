using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paint2API.Models
{
    public partial class ColorDelivery
    {
        public int? IdColorDelivery { get; set; }
        public int? DeliveryId { get; set; }
        public int? ColorId { get; set; }
        [NotMapped]
        public int? Quantity { get; set; }
        public Delivery? Deliverys { get; set; }
        public Color? Colors { get; set; }
    }
}
