using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EPassBook.Models
{
    public class InstallmentListView
    {
        [Key]
        public int InstallmentId { get; set; }
        public int BeneficiaryId { get; set; }
        public string BeneficairyName { get; set; }
        public string MobileNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> PlanYear { get; set; }
        public Nullable<int> StageID { get; set; }
        public Nullable<bool> IsCompleted { get; set; }
        public Nullable<int> InstallmentNo { get; set; }
        public Nullable<int> CompanyID { get; set; }
    }
}