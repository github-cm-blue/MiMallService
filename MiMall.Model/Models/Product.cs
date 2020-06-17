using System;
using System.Collections.Generic;

namespace MiMall.Model.Models
{
    public partial class Product
    {
        public Product()
        {
            Collect = new HashSet<Collect>();
            Orders = new HashSet<Orders>();
            ProductPicture = new HashSet<ProductPicture>();
            ShoppingCart = new HashSet<ShoppingCart>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductIntro { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductSellingPrice { get; set; }
        public int ProductNum { get; set; }
        public int ProductSales { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Collect> Collect { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<ProductPicture> ProductPicture { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}
