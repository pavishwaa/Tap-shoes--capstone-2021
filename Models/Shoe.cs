using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TapShoesCanada.Models
{
    public class Shoe
    {
        public Shoe() 
        { 
        }

        [Required]
        public int Id { get; set; }
       // [DataType(DataType.size)]
        public string Size { get; set; }
        public string Style { get; set; }
        public string Model  { get; set; }
        public string Colour1 { get; set; }
        public string Colour2 { get; set; }
        public string Sole { get; set; }
        public string Lace { get; set; }
        [DataType(DataType.Url)]
        public string Img_Lk { get; set; }
        [DataType(DataType.Currency)]
        public  decimal Price { get; set; }
        public string Model_Type { get; set; }
        // public string Available { get; set; }
    }

   
}
