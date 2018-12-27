using EPassBook.DAL.IService;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static EPassBook.Helper.Common;

namespace EPassBook.Controllers
{
    [RoutePrefix("api/Workflow")]
    public class APIWorkFlowController : ApiController
    {
        IUserService _userService;
        IBenificiaryService _benificiaryService;
        IInstallmentDetailService _installmentDetailService;

        public APIWorkFlowController(IUserService userService, IBenificiaryService benificiaryService, IInstallmentDetailService installmentDetailService)
        {
            _benificiaryService = benificiaryService;
            _userService = userService;
            _installmentDetailService = installmentDetailService;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var beneficiaries = _benificiaryService.GetAllBenificiaries();
            if (beneficiaries != null)
            {
                if (beneficiaries.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, beneficiaries);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Products not found");
        }

        [HttpGet]
        public HttpResponseMessage Get(int beneficiaryId)
        {
            var benificiary = _benificiaryService.GetBenificiaryById(beneficiaryId);
            if (benificiary != null)
                return Request.CreateResponse(HttpStatusCode.OK, benificiary);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Beneficiary found.");
        }

        [HttpGet]
        public HttpResponseMessage GetInstallmentDetails(int beneficiaryId)
        {
            var benificiary = _installmentDetailService.Get(w => w.BeneficiaryId == beneficiaryId, o => o.OrderByDescending(p => p.InstallmentNo), "BenificiaryMaster,Comments,GeoTaggingDetails,InstallmentSignings");
            if (benificiary != null)
                return Request.CreateResponse(HttpStatusCode.OK, benificiary);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Beneficiary found.");
        }

        [HttpGet]
        [Route("Validate/{userName}/{password}")]
        public HttpResponseMessage ValidatedUser(int userName, string password)
        {
            var benificiaryId = _benificiaryService.AuthenticateBeneficiary(userName, password);
            if (benificiaryId != 0)
            {                              
                    return Request.CreateResponse(HttpStatusCode.OK, benificiaryId);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not having access application, Please contact administrator.");
            }
        }

        [HttpGet]
        [Route("Beneficiary/{beneficiaryId}")]
        public HttpResponseMessage GetBeneficiaryDetails(int beneficiaryId)
        {
            var installmentDetail = _installmentDetailService.Get(w => w.BeneficiaryId == beneficiaryId, o => o.OrderByDescending(ob => ob.InstallmentId), "BenificiaryMaster,Comments,GeoTaggingDetails,InstallmentSignings").FirstOrDefault();
            if (installmentDetail != null)
            {
                BeneficiaryAPIViewModel beneficiaryAPIViewModel = new BeneficiaryAPIViewModel();
                beneficiaryAPIViewModel.BeneficiaryId = installmentDetail.BeneficiaryId;
                beneficiaryAPIViewModel.BeneficiaryName = installmentDetail.BenificiaryMaster.BeneficairyName;
                beneficiaryAPIViewModel.MotherName = installmentDetail.BenificiaryMaster.Mother;
                beneficiaryAPIViewModel.FatherName = installmentDetail.BenificiaryMaster.FatherName;
                beneficiaryAPIViewModel.HasbandPhoto = installmentDetail.BenificiaryMaster.Hasband_Photo;
                beneficiaryAPIViewModel.WifePhoto = installmentDetail.BenificiaryMaster.Wife_Photo;
                beneficiaryAPIViewModel.AdharNo = Convert.ToInt32(installmentDetail.BenificiaryMaster.AdharNo);
                beneficiaryAPIViewModel.MobileNo = installmentDetail.BenificiaryMaster.MobileNo;
                beneficiaryAPIViewModel.Address = installmentDetail.BenificiaryMaster.PresentAddress;

                beneficiaryAPIViewModel.IsCompleted = installmentDetail.IsCompleted;
                beneficiaryAPIViewModel.LoanAmnt = installmentDetail.LoanAmnt;
                beneficiaryAPIViewModel.BeneficiaryAmnt = installmentDetail.BeneficiaryAmnt;
                beneficiaryAPIViewModel.CompanyID = installmentDetail.CompanyID;
                beneficiaryAPIViewModel.InstallmentId = installmentDetail.InstallmentId;
                beneficiaryAPIViewModel.InstallmentNo = installmentDetail.InstallmentNo;
                beneficiaryAPIViewModel.ConstructionLevel = installmentDetail.ConstructionLevel;
                beneficiaryAPIViewModel.StageID = installmentDetail.StageID;
                beneficiaryAPIViewModel.IsCentreAmnt = installmentDetail.IsCentreAmnt;
                beneficiaryAPIViewModel.CreatedBy = installmentDetail.CreatedBy;

                beneficiaryAPIViewModel.GeoTaggingDetails = installmentDetail.GeoTaggingDetails.Select(s => new GeoTaggingViewModel
                {
                    Date = s.Date,
                    Photo = "",
                    ConstructionLevel = s.ConstructionLevel,
                    UserId=s.UserId
                }).ToList();

                beneficiaryAPIViewModel.Comments = installmentDetail.Comments.Select(s => new CommentsViewModel
                {
                    Comments = s.Comments,
                    Reason = s.Reason,
                    RoleId = s.RoleId
                }).ToList();

                beneficiaryAPIViewModel.InstallmentSignings = installmentDetail.InstallmentSignings.Select(i => new InstallmentSigningViewModel
                {
                    RoleId = i.RoleId,
                    Sign = i.Sign,
                    CreatedDate = i.CreatedDate
                }
                ).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, beneficiaryAPIViewModel);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not having access application, Please contact administrator.");
            }

        }

        [HttpPost]
        public HttpResponseMessage UpdateInstallmentStatus(int installmentId, int installmentNo)
        {
            var installmentDetail = _installmentDetailService.GetAllInstallmentDetails().Where(w => w.InstallmentId == installmentId && w.InstallmentNo == installmentNo).FirstOrDefault();
            if (installmentDetail != null)
            {
                installmentDetail.StageID = Convert.ToInt32(WorkFlowStages.UserRequest);
                installmentDetail.ModifiedBy = "Beneficiary";
                installmentDetail.ModifiedDate = DateTime.Now;
                _installmentDetailService.Update(installmentDetail);
                _installmentDetailService.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, installmentDetail);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is some problem, Please contact administrator.");
            }
        }
    }
}
