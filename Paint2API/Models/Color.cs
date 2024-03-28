using System;
using System.Collections.Generic;

namespace Paint2API.Models
{
    public partial class Color
    {
        public int? IdColor { get; set; }
        public byte[]? Certificate { get; set; }
        public byte[]? Photo { get; set; }
        public double Priority { get; set; }
        public int? TypeApplicationId { get; set; }
        public int? TempPulverizationId { get; set; }
        public int? ShineId { get; set; }
        public int? TypeSurfaceId { get; set; }
        public int? RalCatalogId { get; set; }
        public TypeApplication? TypeApplications { get; set; }
        public TempPulverization? TempPulverizations { get; set; }
        public Shine? Shines { get; set; }
        public TypeSurface? TypeSurfaces { get; set; }
        public RalCatalog? RalCatalog { get; set; }
    }
}
