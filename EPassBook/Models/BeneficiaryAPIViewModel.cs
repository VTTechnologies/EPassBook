using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class BeneficiaryAPIViewModel
    {

        public int BeneficiaryId { get; set; }
        public int AdharNo { get; set; }
        public string BeneficiaryName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public int InstallmentNo { get; set; }
        public virtual ICollection<InstallmentDetailsViewModel> InstallmentDetails { get; set; }
        public virtual ICollection<CommentsViewModel> Comments { get; set; }
        public virtual ICollection<GeoTaggingViewModel> GeoTaggingDetails { get; set; }
    }
}