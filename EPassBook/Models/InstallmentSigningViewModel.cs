using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class InstallmentSigningViewModel
    {
        [Key]
        public int Id { get; set; }
        public Nullable<int> InstallmentId { get; set; }
        public Nullable<bool> Sign { get; set; }
        public string SignData { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> CompanyID { get; set; }

        public virtual CompanyViewModel CompanyMaster { get; set; }
        public virtual InstallmentDetailsViewModel InstallmentDetail { get; set; }
        public virtual RoleViewModel RoleMaster { get; set; }
        public virtual UserViewModel UserMaster { get; set; }
    }
}