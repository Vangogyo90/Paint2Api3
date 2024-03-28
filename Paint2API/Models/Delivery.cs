using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paint2API.Models
{
    public partial class Delivery
    {
        public int? IdDelivery { get; set; }
        public string? Adress { get; set; }
        public string? Salt { get; set; }
        public byte[]? Cheque { get; set; }
        public int? CityId { get; set; }
        public int? StatusOrderId { get; set; }
        public int? UserId { get; set; }
        public City? Cites { get; set; }
        public User? Users { get; set; }
        [ForeignKey("StatusOrderId")]
        public StatusDelivery? StatusDeliveres { get; set; }
    }
}
