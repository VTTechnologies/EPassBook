using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class InstllmentRejectionViewModel
    {
        public int Id { get; set; }
        public int BeneficiaryId { get; set; }
        public Nullable<int> InstallmentNo { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> CompanyID { get; set; }

        public virtual BeneficiaryViewModel BenificiaryMaster { get; set; }
        public virtual CompanyViewModel CompanyMaster { get; set; }
    }
}