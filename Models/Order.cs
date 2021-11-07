using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TapShoesCanada.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public String CartItems { get; set; }
        public String CustomCartItems { get; set; }
        public String TransactionId { get; set; }
        public String paymentStatus { get; set; }
    }
}
