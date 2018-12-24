using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EPassBook.Models
{
    public class ResetPasswordViewModel
    {
        public int userId { get; set; }

        [DataType(DataType.Password)]
        [Required (ErrorMessage ="please enter old password")]
        public string oldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter new password")]
        public string newPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "please enter confirm password")]
        [Compare("newPassword", ErrorMessage = "Password and Confirmation Password must match.")]
        public string confirmPassword { get; set; }
    }
}