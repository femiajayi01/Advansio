using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advansio.Core.Enums;

namespace Advansio.Core.ViewModels
{
    public class TransferViewModel
    {
        [Required]
        public string? Account { get; set; }
        [Required]
        public string? AccountNumber { get; set; }
        [Required]
        public string? AccountName { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "VAT is required")]
        public decimal Vat { get; set; }
        [Required]
        public Bank? Bank { get; set; }

    }
}
