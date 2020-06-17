using System;
using System.Collections.Generic;

namespace MiMall.Model.Models
{
    public partial class Orders
    {
        public int Id { get; set; }
        public long OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductNum { get; set; }
        public decimal ProductPrice { get; set; }
        public long OrderTime { get; set; }

        public virtual Product Product { get; set; }
        public virtual Users User { get; set; }
    }
}
