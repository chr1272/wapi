using System;
using System.Collections.Generic;

namespace wapi.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Pname { get; set; }
        public DateTime? Entry { get; set; }
        public int? Reportsto { get; set; }
        public int? RoleId { get; set; }
    }
}
