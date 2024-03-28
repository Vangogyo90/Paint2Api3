using System;
using System.Collections.Generic;

namespace Paint2API.Models
{
    public partial class User
    {
        public int? IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string? Password { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Patromymic { get; set; } = null!;
        public string EMail { get; set; } = null!;
        public string NumberTelephone { get; set; } = null!;
        public double Priority { get; set; }
        public string? Salt { get; set; }
        public bool Verification { get; set; }
        public byte[]? Photo { get; set; }
        public int? RoleId { get; set; }
        public Role? Roles { get; set; }
    }
}
