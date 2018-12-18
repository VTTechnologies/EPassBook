using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPassBook.Models
{
    public class CompanyViewModel
    {
        [Key]
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string OwnerName { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> Modifiedby { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual ICollection<BeneficiaryViewModel> BenificiaryMasters { get; set; }
        public virtual ICollection<CommentsViewModel> Comments { get; set; }
        public virtual ICollection<GeoTaggingViewModel> GeoTaggingDetails { get; set; }
        public virtual ICollection<InstallmentDetailsViewModel> InstallmentDetails { get; set; }
        public virtual ICollection<InstllmentRejectionViewModel> InstallmentRejections { get; set; }
        public virtual ICollection<InstallmentSigningViewModel> InstallmentSignings { get; set; }
        public virtual ICollection<UserViewModel> UserMasters { get; set; }
    }
}