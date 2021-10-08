using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TapShoesCanada.Models
{

    enum PaymentStatus
    {
        Pending,
        OnHold,
        Done
    }
    public class Cart
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
        public virtual Shoe Shoe { get; set; }

    }
}
