using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class UserInRoleViewModel
    {
        [Key]
        public int id { get; set; }
        public int RoleId { get; set; }
        public Nullable<int> UserId { get; set; }

        public virtual RoleViewModel RoleMaster { get; set; }
        public virtual UserViewModel UserMaster { get; set; }
    }
}