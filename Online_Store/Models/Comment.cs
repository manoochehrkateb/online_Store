using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Store.Models
{
    public class Comment
    {
        public long Id { get; set; }

        public int ProductIdForComment { get; set; }
        public virtual product Product { get; set; }
        
        public string PostedDate { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
    }
}