using System;
using System.Collections.Generic;

namespace MiMall.Model.Models
{
    public partial class Users
    {
        public Users()
        {
            Carousel = new HashSet<Carousel>();
            Collect = new HashSet<Collect>();
            Orders = new HashSet<Orders>();
            ShoppingCart = new HashSet<ShoppingCart>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Userpwd { get; set; }
        public int RoleId { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public string NickName { get; set; }

        public virtual UserRole Role { get; set; }
        public virtual ICollection<Carousel> Carousel { get; set; }
        public virtual ICollection<Collect> Collect { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}
