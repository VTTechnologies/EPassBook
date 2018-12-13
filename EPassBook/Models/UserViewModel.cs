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

        [Required(ErrorMessage = "Please enter the user's User name.")]
        [StringLength(50, ErrorMessage = "The First Name must be less than {1} characters.")]
        [Display(Name = "User Name:")]
        public string UserName { get; set; }

        public string Password { get; set; }
        public Nullable<bool> IsActive { get; set; }
        //public Nullable<bool> IsLoggedIn { get; set; }
        public int RoleId { get; set; }

        //public List<RoleViewModel>
    }
}