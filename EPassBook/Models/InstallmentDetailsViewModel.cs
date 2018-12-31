using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPassBook.Models
{
    public class InstallmentDetailsViewModel
    {
        [Key]
        public int InstallmentId { get; set; }
        public int BeneficiaryId { get; set; }
        [Range(0, 100000000000)]
        public Nullable<decimal> BeneficiaryAmnt { get; set; }
        [Required(ErrorMessage ="Please enter Loan Amount")]
        [Range(0,100000000000)]
        public Nullable<decimal> LoanAmnt { get; set; }
        public Nullable<bool> IsCentreAmnt { get; set; }
        public string ConstructionLevel { get; set; }
        public Nullable<int> StageID { get; set; }
        public Nullable<bool> IsCompleted { get; set; }
        public Nullable<int> InstallmentNo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string _Comments { get; set; }
        public string Photo { get; set; }
        public Nullable<decimal> TransactionID { get; set; }
        public string lInRupees { get; set; }
        public string beniInRupees { get; set; }


        public virtual BeneficiaryViewModel BenificiaryMaster { get; set; }
        public virtual ICollection<CommentsViewModel> Comments { get; set; }
        public virtual CompanyViewModel CompanyMaster { get; set; }
        public virtual ICollection<GeoTaggingViewModel> GeoTaggingDetails { get; set; }
        public virtual ICollection<InstallmentSigningViewModel> InstallmentSignings { get; set; }
    }
}