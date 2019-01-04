﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPassBook.Helper;

namespace EPassBook.Models
{
    public class UserViewModel
    {
        [Key]
        public int UserId { get; set; }

        //[Required(ErrorMessage = "User Name is required.")]
        [StringLength(50, ErrorMessage = "The First Name must be less than {1} characters.")]
        [Display(Name = "User Name:")]
        public string UserName { get; set; }
        //[Required(ErrorMessage = "Please enter the Password.")]
        public string Password { get; set; }
        [NotMapped]
        public string DecryptedPass
        {
            get
            {
                return Password.Decrypt();
            }
            set
            {
                Password = value.Encrypt();
            }
        }
        [Display(Name = "Active:")]
        public bool? IsActive { get; set;}
        //public Nullable<bool> IsLoggedIn { get; set; }
        [Required(ErrorMessage = "Please select user role.")]
        public int RoleId { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile No is required.")]
        public string MobileNo { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Please select City.")]
        public Nullable<int> CityId { get; set; }
        [Required(ErrorMessage = "Please select Company.")]
        public Nullable<int> CompanyID { get; set; }
        public bool RememberMe { get; set; }
      
        public bool? IsReset { get; set; }
        public string CityName { get; set; }
        [Display(Name ="Role")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Date of birth is required.")]

        public Nullable<System.DateTime> Dob { get; set; }

        public virtual CityViewModel CityMaster { get; set; }
        public virtual CompanyViewModel CompanyMaster { get; set; }
        public virtual ICollection<GeoTaggingViewModel> GeoTaggingDetails { get; set; }
        public virtual ICollection<InstallmentSigningViewModel> InstallmentSignings { get; set; }     
        public virtual ICollection<UserInRoleViewModel> UserInRoles { get; set; }

        public IEnumerable<SelectList> AllRoles { set; get; }
        public IEnumerable<SelectList> Cities { set; get; }
        public IEnumerable<SelectList> Comanies { set; get; }

        public bool? IsLoggedIn { get; set; }
       

      
     
       
    }
}