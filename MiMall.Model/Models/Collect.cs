using System;
using System.Collections.Generic;

namespace MiMall.Model.Models
{
    public partial class Collect
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public long CollectTime { get; set; }

        public virtual Product Product { get; set; }
        public virtual Users User { get; set; }
    }
}
