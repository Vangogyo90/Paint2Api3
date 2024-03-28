using System;
using System.Collections.Generic;

namespace Paint2API.Models
{
    public partial class Discount
    {
        public int? IdDiscount { get; set; }
        public int? ColorId { get; set; }
        public int SizeDicsount { get; set; }
        public Color? Colors { get; set; }
    }
}
