using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TapShoesCanada.Models
{
    public class CustomCart
    {
        public List<Cart> Carts { get; set; }
        public List<CartCustomShoe> CustomCarts { get; set; }
    }
}
