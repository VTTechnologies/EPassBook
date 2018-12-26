using EPassBook.DAL.DBModel;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Mapper
{
    public static class InstallmentDetailsMapper
    {

        public static InstallmentDetail Attach(InstallmentDetailsViewModel installmentDetailsViewModel)
        {
            InstallmentDetail installmentDetail = new InstallmentDetail();

            installmentDetail.InstallmentId = installmentDetailsViewModel.InstallmentId;
            installmentDetail.BeneficiaryId = installmentDetailsViewModel.BeneficiaryId;
            installmentDetail.BeneficiaryAmnt = installmentDetailsViewModel.BeneficiaryAmnt;
            installmentDetail.LoanAmnt = installmentDetailsViewModel.LoanAmnt;
            installmentDetail.IsCentreAmnt = installmentDetailsViewModel.IsCentreAmnt;
            installmentDetail.ConstructionLevel = installmentDetailsViewModel.ConstructionLevel;
            installmentDetail.StageID = installmentDetailsViewModel.StageID;
            installmentDetail.IsCompleted = installmentDetailsViewModel.IsCompleted;
            installmentDetail.InstallmentNo = installmentDetailsViewModel.InstallmentNo;
            installmentDetail.CreatedDate = installmentDetailsViewModel.CreatedDate;
            installmentDetail.CreatedBy = installmentDetailsViewModel.CreatedBy;
            installmentDetail.ModifiedDate = installmentDetailsViewModel.ModifiedDate;
            installmentDetail.ModifiedBy = installmentDetailsViewModel.ModifiedBy;
            installmentDetail.CompanyID = installmentDetailsViewModel.CompanyID;
            installmentDetail.TransactionID = installmentDetailsViewModel.TransactionID;

            installmentDetail.Comments = installmentDetailsViewModel.Comments.Select(s => new Comment() {  
                Id = s.Id,
                BeneficiaryId = s.BeneficiaryId,
                InstallementId = s.InstallementId,
                Reason = s.Reason,
                Comments = s.Comments,
                RoleId = s.RoleId,
                CreatedDate = s.CreatedDate,
                CreatedBy = s.CreatedBy,
                CompanyID = s.CompanyID            
            }).ToList();

            installmentDetail.GeoTaggingDetails = installmentDetailsViewModel.GeoTaggingDetails.Select(g => new GeoTaggingDetail() {
                Id = g.Id,
                Photo = g.Photo,
                ConstructionLevel = g.ConstructionLevel,
                BeneficiaryId = g.BeneficiaryId,
                UserId = g.UserId,
                InstallmentId = g.InstallmentId,
                Date = g.Date,
                CreatedDate = g.CreatedDate,
                CreatedBy = g.CreatedBy,
                ModifiedDate = g.ModifiedDate,
                ModifiedBy = g.ModifiedBy,
                CompanyID = g.CompanyID,
            }).ToList();

            installmentDetail.InstallmentSignings = installmentDetailsViewModel.InstallmentSignings.Select(d => new InstallmentSigning() {
                Id = d.Id,
                InstallmentId = d.InstallmentId,
                Sign = d.Sign,
                SignData = d.SignData,
                UserId = d.UserId,
                RoleId = d.RoleId,
                CreatedDate = d.CreatedDate,
                CreatedBy = d.CreatedBy,
                ModifiedDate = d.ModifiedDate,
                ModifiedBy = d.ModifiedBy,
                CompanyID = d.CompanyID
            }).ToList();

            return installmentDetail;
        }
        public static InstallmentDetailsViewModel Detach(InstallmentDetail installmentDetail)
        {
            InstallmentDetailsViewModel installmentDetailsViewModel = new InstallmentDetailsViewModel();

            installmentDetailsViewModel.InstallmentId = installmentDetail.InstallmentId;
            installmentDetailsViewModel.BeneficiaryId = installmentDetail.BeneficiaryId;
            installmentDetailsViewModel.BeneficiaryAmnt = installmentDetail.BeneficiaryAmnt;
            installmentDetailsViewModel.LoanAmnt = installmentDetail.LoanAmnt;
            installmentDetailsViewModel.IsCentreAmnt = installmentDetail.IsCentreAmnt;
            installmentDetailsViewModel.ConstructionLevel = installmentDetail.ConstructionLevel;
            installmentDetailsViewModel.StageID = installmentDetail.StageID;
            installmentDetailsViewModel.IsCompleted = installmentDetail.IsCompleted;
            installmentDetailsViewModel.InstallmentNo = installmentDetail.InstallmentNo;
            installmentDetailsViewModel.CreatedDate = installmentDetail.CreatedDate;
            installmentDetailsViewModel.CreatedBy = installmentDetail.CreatedBy;
            installmentDetailsViewModel.ModifiedDate = installmentDetail.ModifiedDate;
            installmentDetailsViewModel.ModifiedBy = installmentDetail.ModifiedBy;
            installmentDetailsViewModel.CompanyID = installmentDetail.CompanyID;
            installmentDetailsViewModel.TransactionID = installmentDetail.TransactionID;

            installmentDetailsViewModel.Comments = installmentDetail.Comments.Select(s => new CommentsViewModel()
            {
                Id = s.Id,
                BeneficiaryId = s.BeneficiaryId,
                InstallementId = s.InstallementId,
                Reason = s.Reason,
                Comments = s.Comments,
                RoleId = s.RoleId,
                CreatedDate = s.CreatedDate,
                CreatedBy = s.CreatedBy,
                CompanyID = s.CompanyID,                
            }).ToList();

            installmentDetailsViewModel.GeoTaggingDetails = installmentDetail.GeoTaggingDetails.Select(g => new GeoTaggingViewModel()
            {
                Id = g.Id,
                Photo = g.Photo,
                ConstructionLevel = g.ConstructionLevel,
                BeneficiaryId = g.BeneficiaryId,
                UserId = g.UserId,
                InstallmentId = g.InstallmentId,
                Date = g.Date,
                CreatedDate = g.CreatedDate,
                CreatedBy = g.CreatedBy,
                ModifiedDate = g.ModifiedDate,
                ModifiedBy = g.ModifiedBy,
                CompanyID = g.CompanyID,
            }).ToList();

            installmentDetailsViewModel.InstallmentSignings = installmentDetail.InstallmentSignings.Select(d => new InstallmentSigningViewModel()
            {
                Id = d.Id,
                InstallmentId = d.InstallmentId,
                Sign = d.Sign,
                SignData = d.SignData,
                UserId = d.UserId,
                RoleId = d.RoleId,
                CreatedDate = d.CreatedDate,
                CreatedBy = d.CreatedBy,
                ModifiedDate = d.ModifiedDate,
                ModifiedBy = d.ModifiedBy,
                CompanyID = d.CompanyID
            }).ToList();

            installmentDetailsViewModel._Comments = installmentDetailsViewModel.Comments.Select(d => d.Comments).FirstOrDefault();
            installmentDetailsViewModel.Photo = installmentDetailsViewModel.GeoTaggingDetails.Select(p => p.Photo).FirstOrDefault();

            return installmentDetailsViewModel;
        }
    }
}