using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class CityHeadViewModel
    {
        public int CityHeadDetailId { get; set; }
        public int BeneficiaryId { get; set; }
        public Nullable<decimal> CentralAmnt { get; set; }
        public Nullable<decimal> StateAmnt { get; set; }
        public Nullable<bool> CHSign { get; set; }
        public string CHSignData { get; set; }
        public Nullable<int> InstallmentNo { get; set; }
        public Nullable<bool> CESign { get; set; }
        public string CESignData { get; set; }
        public Nullable<bool> COSign { get; set; }
        public string COSignData { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public virtual BeneficiaryViewModel BenificiaryMaster { get; set; }
        public virtual UserViewModel UserMaster { get; set; }
    }
}