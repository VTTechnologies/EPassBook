using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class CommentsViewModel
    {
        public int Id { get; set; }
        public int BeneficiaryId { get; set; }
        public int InstallementId { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<int> CompanyID { get; set; }

        public virtual BeneficiaryViewModel BenificiaryMaster { get; set; }
        public virtual CompanyViewModel CompanyMaster { get; set; }
        public virtual InstallmentDetailsViewModel InstallmentDetail { get; set; }
        public virtual RoleViewModel RoleMaster { get; set; }
    }
}