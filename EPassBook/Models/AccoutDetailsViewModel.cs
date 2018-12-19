using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class AccountDetailsViewModel
    {
        [Key]
        public long LoanAmnt { get; set; }
        public string LoanAmtInRupees { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNo { get; set; }
        public int BenifciaryId { get; set; }
        public bool Sign { get; set; }
        public string TransactionId { get; set; }
        public int InstallmentId { get; set; }
        public int UserId { get; set; }
    }
}