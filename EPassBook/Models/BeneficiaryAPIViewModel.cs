using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class BeneficiaryAPIViewModel
    {

        public int BeneficiaryId { get; set; }
        public string BeneficiaryName { get; set; }
        public string HasbandPhoto { get; set; }
        public string WifePhoto { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public int AdharNo { get; set; }        
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public int? InstallmentNo { get; set; }
        public decimal? BeneficiaryAmnt { get; set; }
        public decimal? LoanAmnt { get; set; }
        public bool? IsCentreAmnt { get; set; }
        public string ConstructionLevel { get; set; }
        public int? StageID { get; set; }
        public bool? IsCompleted { get; set; }
        public int InstallmentId { get; set; }
        public int? CompanyID { get; set; }
        public string CreatedBy { get; set; }


        public virtual ICollection<CommentsViewModel> Comments { get; set; }
        public virtual ICollection<GeoTaggingViewModel> GeoTaggingDetails { get; set; }
        public virtual ICollection<InstallmentSigningViewModel> InstallmentSignings { get; set; }
    }
}