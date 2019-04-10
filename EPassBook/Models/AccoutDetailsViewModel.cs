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
        [Range(typeof(bool), "true", "true", ErrorMessage = "Please sign before submit")]
        public bool Sign { get; set; }
        [Required(ErrorMessage = "Transaction Id is required")]
        public string TransactionId { get; set; }
        public int InstallmentId { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string TransactionType { get; set; }
        [MaxLength(4)]
        [Required(ErrorMessage = "OTP is Required")]
        public string OTP { get; set; }
    }
}