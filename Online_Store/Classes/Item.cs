using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_Store.Models;

namespace Online_Store.Classes
{
    public struct DDL
    {
        public int Value { get; set; }
        public String Text { get; set; }
    }
    public class Item
    {
        private product pr = new product();
        public int Quantity { get;  set; }
        
        public product Pr
        {
            get { return pr; }
            set { pr = value; }
        }

        public Item()
        { }

        public Item(product product, int quantity)
        {
            this.pr = product;
            this.Quantity = quantity;
        }


    }
}