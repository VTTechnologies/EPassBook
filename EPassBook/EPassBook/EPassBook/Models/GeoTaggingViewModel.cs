using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class GeoTaggingViewModel
    {
        public long Id { get; set; }
        public byte[] Photo { get; set; }
        public string ConstructionLevel { get; set; }
        public Nullable<int> BeneficiaryId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> Installment { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public  BeneficiaryViewModel BenificiaryMaster { get; set; }
        public  UserViewModel UserMaster { get; set; }
    }
}