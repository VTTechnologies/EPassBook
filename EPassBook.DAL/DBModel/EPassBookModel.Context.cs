﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EPassBookEntities : DbContext
    {
        public EPassBookEntities()
            : base("name=EPassBookEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BenificiaryMaster> BenificiaryMasters { get; set; }
        public virtual DbSet<CityMaster> CityMasters { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CommentsHisory> CommentsHisories { get; set; }
        public virtual DbSet<CompanyMaster> CompanyMasters { get; set; }
        public virtual DbSet<GeoTaggingDetail> GeoTaggingDetails { get; set; }
        public virtual DbSet<InstallmentDetail> InstallmentDetails { get; set; }
        public virtual DbSet<InstallmentDetailsHistory> InstallmentDetailsHistories { get; set; }
        public virtual DbSet<InstallmentRejection> InstallmentRejections { get; set; }
        public virtual DbSet<InstallmentSigning> InstallmentSignings { get; set; }
        public virtual DbSet<RoleMaster> RoleMasters { get; set; }
        public virtual DbSet<StageInRole> StageInRoles { get; set; }
        public virtual DbSet<UserInRole> UserInRoles { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<WorkflowStage> WorkflowStages { get; set; }
    }
}
