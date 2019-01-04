using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class WorkStatusDetailsViewModel
    {
        public string Installment { get; set; }
        public string LevelType { get; set; }
        public decimal? StateAmount { get; set; }
        public decimal? CenterAmount { get; set; }
        public decimal? BeneficiaryAmount { get; set; }
        public decimal ULBAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? GrandTotal { get; set; }
    }
}