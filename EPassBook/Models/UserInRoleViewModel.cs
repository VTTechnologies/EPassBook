using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class UserInRoleViewModel
    {
        public int id { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> UserId { get; set; }

        public virtual RoleViewModel RoleMaster { get; set; }
        public virtual UserViewModel UserMaster { get; set; }
    }
}