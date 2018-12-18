using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class CityViewModel
    {
        [Key]
        public int CityID { get; set; }
        public string CityName { get; set; }
        public string CityShortName { get; set; } 
        public Nullable<bool> IsActive { get; set; }

        public virtual ICollection<BeneficiaryViewModel> BenificiaryMasters { get; set; }                                        
        public virtual ICollection<UserViewModel> UserMasters { get; set; }
    }
}