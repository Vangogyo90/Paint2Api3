using System;
using System.Collections.Generic;

namespace Paint2API.Models
{
    public partial class Token
    {
        public int? IdToken { get; set; }
        public string? Token1 { get; set; }
        public int? UserId { get; set; }
        public User? Users { get; set; }
    }
}
