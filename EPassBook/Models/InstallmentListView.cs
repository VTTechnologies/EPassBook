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

        [Display(Name = "Beneficiary Id")]
        public int BeneficiaryId { get; set; }
        [Display(Name = "Beneficairy Name")]
        public string BeneficairyName { get; set; }
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }
        [Display(Name = "Plan Year")]
        public int planYear { get; set; }

        [Display(Name = "Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Plan Year")]
        public Nullable<int> PlanYear { get; set; }
        public Nullable<int> StageID { get; set; }
        public Nullable<bool> IsCompleted { get; set; }

        [Display(Name = "Installment No")]
        public Nullable<int> InstallmentNo { get; set; }
        public Nullable<int> CompanyID { get; set; }
    }
}