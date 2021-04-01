using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TapShoesCanada.Models
{
    public class Shoe
    {
        public int Id { get; set; }
        public Decimal Size { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
    }
}
