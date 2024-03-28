using System;
using System.Collections.Generic;

namespace Paint2API.Models
{
    public partial class Shine
    {
        public int? IdShine { get; set; }
        public int FirstProcent { get; set; }
        public int EndProcent { get; set; }
        public string NameShine { get; set; } = null!;
    }
}
