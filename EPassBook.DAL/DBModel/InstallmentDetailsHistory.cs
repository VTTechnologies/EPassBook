//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPassBook.DAL.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class InstallmentDetailsHistory
    {
        public int id { get; set; }
        public int InstallmentId { get; set; }
        public int BeneficiaryId { get; set; }
        public Nullable<decimal> BeneficiaryAmnt { get; set; }
        public Nullable<decimal> LoanAmnt { get; set; }
        public Nullable<bool> IsCentreAmnt { get; set; }
        public string ConstructionLevel { get; set; }
        public Nullable<int> StageID { get; set; }
        public Nullable<bool> IsCompleted { get; set; }
        public Nullable<int> InstallmentNo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<bool> IsRecommended { get; set; }
    }
}
