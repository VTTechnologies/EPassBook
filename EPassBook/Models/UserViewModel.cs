using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class UserViewModel
    {


        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter the User name.")]
        [StringLength(50, ErrorMessage = "The First Name must be less than {1} characters.")]
        [Display(Name = "User Name:")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter the Password.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string Email { get; set; }
        public Nullable<decimal> MobileNo { get; set; }
        public string Address { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsLoggedIn { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> CompanyID { get; set; }

        public virtual CityViewModel CityMaster { get; set; }
        public virtual CompanyViewModel CompanyMaster { get; set; }
        public virtual ICollection<GeoTaggingViewModel> GeoTaggingDetails { get; set; }
        public virtual ICollection<InstallmentSigningViewModel> InstallmentSignings { get; set; }
        public virtual ICollection<UserInRoleViewModel> UserInRoles { get; set; }

    }
}