using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class BeneficiaryViewModel
    {
        [Key]
        public int BeneficiaryId { get; set; }
        [Required(ErrorMessage = "Beneficairy Name is required.")]
        public string BeneficairyName { get; set; }
        [Required(ErrorMessage = "Father Name is required.")]
        public string FatherName { get; set; }
        public string Mother { get; set; }
        [Required(ErrorMessage = "MobileNo is required.")]
        public string MobileNo { get; set; }
        public string PresentAddress { get; set; }
        public Nullable<int> CityId { get; set; }
        public string DTRNo { get; set; }
        public Nullable<long> RecordNo { get; set; }
        public string Class { get; set; }
        public string General { get; set; }
        public string Single { get; set; }
        public string Disabled { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Adhar No is required.")]
        public Nullable<long> AdharNo { get; set; }
        public string VoterID { get; set; }
        public Nullable<int> Area { get; set; }
        public Nullable<int> MojaNo { get; set; }
        public string KhataNo { get; set; }
        public Nullable<int> KhasraNo { get; set; }
        public string PlotNo { get; set; }
        public string PoliceStation { get; set; }
        public string WardNo { get; set; }
        public string District { get; set; }
        public string BankName { get; set; }
        [Required(ErrorMessage = "Branch Name is required.")]
        public string BranchName { get; set; }
        [Required(ErrorMessage = "IFSC Code is required.")]
        public string IFSCCode { get; set; }
        [Required(ErrorMessage = "Account No is required.")]
        public string AccountNo { get; set; }
        public string Hasband_Photo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string Wife_Photo { get; set; }
        public int installmentId { get; set; }
        public virtual CompanyViewModel CompanyMaster { get; set; }
        public virtual CityViewModel CityMaster { get; set; }
        public virtual ICollection<InstallmentDetailsViewModel> InstallmentDetails { get; set; }
        public virtual ICollection<CommentsViewModel> Comments { get; set; }
        public virtual ICollection<GeoTaggingViewModel> GeoTaggingDetails { get; set; }
        public virtual ICollection<InstllmentRejectionViewModel> InstallmentRejections { get; set; }    

       
    }
}