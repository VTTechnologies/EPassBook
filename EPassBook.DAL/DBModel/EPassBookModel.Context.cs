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
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
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
        public virtual DbSet<InstallmentRejection> InstallmentRejections { get; set; }
        public virtual DbSet<InstallmentSigning> InstallmentSignings { get; set; }
        public virtual DbSet<RoleMaster> RoleMasters { get; set; }
        public virtual DbSet<StageInRole> StageInRoles { get; set; }
        public virtual DbSet<UserInRole> UserInRoles { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<WorkflowStage> WorkflowStages { get; set; }
        public virtual DbSet<ELMAH_Error> ELMAH_Error { get; set; }
        public virtual DbSet<InstallmentDetailsHistory> InstallmentDetailsHistories { get; set; }
        public virtual DbSet<InstallmentDetail> InstallmentDetails { get; set; }
    
        public virtual ObjectResult<string> ELMAH_GetErrorsXml(string application, Nullable<int> pageIndex, Nullable<int> pageSize, ObjectParameter totalCount)
        {
            var applicationParameter = application != null ?
                new ObjectParameter("Application", application) :
                new ObjectParameter("Application", typeof(string));
    
            var pageIndexParameter = pageIndex.HasValue ?
                new ObjectParameter("PageIndex", pageIndex) :
                new ObjectParameter("PageIndex", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ELMAH_GetErrorsXml", applicationParameter, pageIndexParameter, pageSizeParameter, totalCount);
        }
    
        public virtual ObjectResult<string> ELMAH_GetErrorXml(string application, Nullable<System.Guid> errorId)
        {
            var applicationParameter = application != null ?
                new ObjectParameter("Application", application) :
                new ObjectParameter("Application", typeof(string));
    
            var errorIdParameter = errorId.HasValue ?
                new ObjectParameter("ErrorId", errorId) :
                new ObjectParameter("ErrorId", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ELMAH_GetErrorXml", applicationParameter, errorIdParameter);
        }
    
        public virtual int ELMAH_LogError(Nullable<System.Guid> errorId, string application, string host, string type, string source, string message, string user, string allXml, Nullable<int> statusCode, Nullable<System.DateTime> timeUtc)
        {
            var errorIdParameter = errorId.HasValue ?
                new ObjectParameter("ErrorId", errorId) :
                new ObjectParameter("ErrorId", typeof(System.Guid));
    
            var applicationParameter = application != null ?
                new ObjectParameter("Application", application) :
                new ObjectParameter("Application", typeof(string));
    
            var hostParameter = host != null ?
                new ObjectParameter("Host", host) :
                new ObjectParameter("Host", typeof(string));
    
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var sourceParameter = source != null ?
                new ObjectParameter("Source", source) :
                new ObjectParameter("Source", typeof(string));
    
            var messageParameter = message != null ?
                new ObjectParameter("Message", message) :
                new ObjectParameter("Message", typeof(string));
    
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var allXmlParameter = allXml != null ?
                new ObjectParameter("AllXml", allXml) :
                new ObjectParameter("AllXml", typeof(string));
    
            var statusCodeParameter = statusCode.HasValue ?
                new ObjectParameter("StatusCode", statusCode) :
                new ObjectParameter("StatusCode", typeof(int));
    
            var timeUtcParameter = timeUtc.HasValue ?
                new ObjectParameter("TimeUtc", timeUtc) :
                new ObjectParameter("TimeUtc", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ELMAH_LogError", errorIdParameter, applicationParameter, hostParameter, typeParameter, sourceParameter, messageParameter, userParameter, allXmlParameter, statusCodeParameter, timeUtcParameter);
        }
    
        public virtual ObjectResult<sp_GetSurveyDetailsByBenID_Result> sp_GetSurveyDetailsByBenID(Nullable<int> benificiaryId)
        {
            var benificiaryIdParameter = benificiaryId.HasValue ?
                new ObjectParameter("BenificiaryId", benificiaryId) :
                new ObjectParameter("BenificiaryId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetSurveyDetailsByBenID_Result>("sp_GetSurveyDetailsByBenID", benificiaryIdParameter);
        }
    
        public virtual int sp_UpdatTransactionId(Nullable<int> benifciaryId, Nullable<bool> sign, Nullable<decimal> transactionId, Nullable<int> installmentId, Nullable<int> userid)
        {
            var benifciaryIdParameter = benifciaryId.HasValue ?
                new ObjectParameter("benifciaryId", benifciaryId) :
                new ObjectParameter("benifciaryId", typeof(int));
    
            var signParameter = sign.HasValue ?
                new ObjectParameter("sign", sign) :
                new ObjectParameter("sign", typeof(bool));
    
            var transactionIdParameter = transactionId.HasValue ?
                new ObjectParameter("transactionId", transactionId) :
                new ObjectParameter("transactionId", typeof(decimal));
    
            var installmentIdParameter = installmentId.HasValue ?
                new ObjectParameter("installmentId", installmentId) :
                new ObjectParameter("installmentId", typeof(int));
    
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_UpdatTransactionId", benifciaryIdParameter, signParameter, transactionIdParameter, installmentIdParameter, useridParameter);
        }
    
        public virtual ObjectResult<sp_GetInstallmentListViewForUsersRoles_Result> sp_GetInstallmentListViewForUsersRoles(Nullable<int> stageid)
        {
            var stageidParameter = stageid.HasValue ?
                new ObjectParameter("stageid", stageid) :
                new ObjectParameter("stageid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetInstallmentListViewForUsersRoles_Result>("sp_GetInstallmentListViewForUsersRoles", stageidParameter);
        }
    }
}
