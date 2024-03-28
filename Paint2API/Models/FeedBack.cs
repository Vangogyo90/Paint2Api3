using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paint2API.Models
{
    public partial class FeedBack
    {
        public int? IdFeedBack { get; set; }
        public string Number_Or_E_mail { get; set; } = null!;
        public bool? End { get; set; }
        public string NameUser { get; set; } = null!;
        public byte[]? Message { get; set; }
        public int? UserId { get; set; }
        public User? Users { get; set; }
        public DateTime? Date {get; set;}
        public string Category { get; set; } = null!;
    }
}
