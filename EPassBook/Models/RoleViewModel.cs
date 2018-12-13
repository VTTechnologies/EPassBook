using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual ICollection<CommentsViewModel> Comments { get; set; }
        public virtual ICollection<InstallmentSigningViewModel> InstallmentSignings { get; set; }
        public virtual ICollection<StageinRoleViewModel> StageInRoles { get; set; }
        public virtual ICollection<UserViewModel> UserMasters { get; set; }
        public virtual ICollection<UserInRoleViewModel> UserInRoles { get; set; }
    }
}