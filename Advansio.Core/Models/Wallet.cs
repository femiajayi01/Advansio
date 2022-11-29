using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advansio.Core.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        public decimal WalletBalance { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
