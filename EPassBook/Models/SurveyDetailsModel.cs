using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class SurveyDetailsModel
    {
        public int BeneficiaryId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string Physical_Progress { get; set; }
        public string Comments { get; set; }
        public bool? Sign { get; set; }
    }
}