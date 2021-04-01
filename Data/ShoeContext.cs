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
        { }


        public DbSet<Shoe> Shoes { get; set; }
    }

}
