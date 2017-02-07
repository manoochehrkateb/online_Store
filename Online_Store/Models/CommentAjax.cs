using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Net.Mime;
using System.Web;

namespace Online_Store.Models
{
    //public class 
    public class CommentAjax
    {

        public CommentAjax()
        {
            CommentAjax1 = new HashSet<CommentAjax>();
        }

        [Key]
        public int CommentID { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int? ParentID { get; set; }
        public virtual ICollection<CommentAjax> CommentAjax1 { get; set; }

        public virtual CommentAjax CommentAjax2 { get; set; }
    }
}