using System;
using System.Collections.Generic;

namespace wapi.Models
{
    public partial class Comment

    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Lastmodified { get; set; }
        public int? ParentCommentId { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? SiteId { get; set; }
        public string DocumentId { get; set; }
    }
}
