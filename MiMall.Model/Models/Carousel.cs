using System;
using System.Collections.Generic;

namespace MiMall.Model.Models
{
    public partial class Carousel
    {
        public int CarouselId { get; set; }
        public int UserId { get; set; }
        public string ImgPath { get; set; }
        public string Describes { get; set; }

        public virtual Users User { get; set; }
    }
}
