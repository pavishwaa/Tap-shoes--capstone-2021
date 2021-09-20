using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TapShoesCanada.Models;
using Microsoft.EntityFrameworkCore;

namespace TapShoesCanada.Data
{
    public class CustomShoeContext : DbContext
    {

        public CustomShoeContext(DbContextOptions<CustomShoeContext> options) : base(options)
        {}


        public DbSet<Custom_shoe> Custom_Shoes { get; set; }
    }
}
