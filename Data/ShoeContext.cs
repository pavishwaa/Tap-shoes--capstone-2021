using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TapShoesCanada.Models;
using Microsoft.EntityFrameworkCore;

namespace TapShoesCanada.Data
{
    public class ShoeContext : DbContext
    {
        public ShoeContext(DbContextOptions<ShoeContext> options) : base(options)
        {
            
        }


        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartCustomShoe> CartCustomShoes { get; set; } 
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Custom_shoe> Custom_Shoes { get; set; }
    }

}
