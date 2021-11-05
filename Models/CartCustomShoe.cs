using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TapShoesCanada.Models
{
    public class CartCustomShoe
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }
        public int CartUserId { get; set; }

        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public String PaymentStatus { get; set; }

        public int ShoeId { get; set; }
        public virtual Custom_shoe Shoe { get; set; }

    }
}
