using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advansio.Core.ViewModels
{
    public class TransactionViewModel
    {
        [Required]
        public decimal Amount { get; set; }
    
       
    }
}
