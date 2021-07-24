using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TapShoesCanada.Models
{
    public class Shoe
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string Style { get; set; }
        public int Model  { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Sole { get; set; }
        public string Lace { get; set; }
        public string Img_Lk { get; set; }
        public int Price { get; set; }
        public string Model_Type { get; set; }
        // public string Available { get; set; }
    }
}
