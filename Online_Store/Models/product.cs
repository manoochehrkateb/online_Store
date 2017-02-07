using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Store.Models
{
    public partial class Type
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
    public partial class Cat
    {
        public int CatId { get; set; }
        public string CatName { get; set; }
        public int TypeId { get; set; }
    }
    public class product
    {
        public int productID { get; set; }
        public string name { get; set; }
        [AllowHtml]
        public string Desciption { get; set; }
        [DataType(DataType.Currency)]
        public decimal price { get; set; }

//        public int quanitity { get; set; }
        public DateTime? date { get; set; }
        public int stock { get; set; }
        public int type { get; set; }
        public int catgorie { get; set; }
        public ICollection<productImage> productImage { get; set; }
        //public virtual ICollection<product> productpage { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
    public class RecentProduct
    {
        public int ProductId { get; set; }

        public string ProdutName { get; set; }

        public DateTime LastVisited { get; set; }
    }
}