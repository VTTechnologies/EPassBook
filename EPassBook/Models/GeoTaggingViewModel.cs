using System;
using System.ComponentModel.DataAnnotations;

namespace EPassBook.Models
{
    public class GeoTaggingViewModel
    {
        [Key]
        public long Id { get; set; }
        public byte[] Photo { get; set; }
        public string ConstructionLevel { get; set; }
        public Nullable<int> BeneficiaryId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> InstallmentId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> CompanyID { get; set; }

        public virtual BeneficiaryViewModel BenificiaryMaster { get; set; }
        public virtual CompanyViewModel CompanyMaster { get; set; }
        public virtual InstallmentDetailsViewModel InstallmentDetail { get; set; }
        public virtual UserViewModel UserMaster { get; set; }
    }
}