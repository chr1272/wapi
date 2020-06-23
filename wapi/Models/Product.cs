using System;
using System.Collections.Generic;

namespace wapi.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int ConfigitemId { get; set; }
        public string ProductType { get; set; }
        public int? ParentItemId { get; set; }
    }
}
