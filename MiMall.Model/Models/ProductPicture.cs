using System;
using System.Collections.Generic;

namespace MiMall.Model.Models
{
    public partial class ProductPicture
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductPicture1 { get; set; }
        public string Intro { get; set; }

        public virtual Product Product { get; set; }
    }
}
