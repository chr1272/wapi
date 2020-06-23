using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wapi.Models
{
    public class userdoc
    {
        public int Id { get; set; }
        public string Folder { get; set; }
        public string Title { get; set; }
        public int Version { get; set; }
        public string Psr { get; set; }
        public string Indexfile { get; set; }
    }
}
