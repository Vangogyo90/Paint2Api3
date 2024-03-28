using System;
using System.Collections.Generic;

namespace Paint2API.Models
{
    public partial class News
    {
        public int? IdNews { get; set; }
        public string HeadingNews { get; set; } = null!;
        public byte[] TextNews { get; set; } = null!;
        public DateTime? Date { get; set; }
        public int? UserId { get; set; }
        public User? Users { get; set; }
    }
}
