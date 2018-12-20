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
    
    public partial class GeoTaggingDetail
    {
        public long Id { get; set; }
        public byte[] Photo { get; set; }
        public string ConstructionLevel { get; set; }
        public Nullable<int> BeneficiaryId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> InstallmentId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> CompanyID { get; set; }
    
        public virtual BenificiaryMaster BenificiaryMaster { get; set; }
        public virtual CompanyMaster CompanyMaster { get; set; }
        public virtual InstallmentDetail InstallmentDetail { get; set; }
        public virtual UserMaster UserMaster { get; set; }
    }
}
