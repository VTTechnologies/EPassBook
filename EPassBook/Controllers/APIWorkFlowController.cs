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
            var benificiary = _installmentDetailService.Get(w=>w.BeneficiaryId==beneficiaryId,o=>o.OrderByDescending(p=>p.InstallmentNo), "BenificiaryMaster,Comments,GeoTaggingDetails,InstallmentSignings");
            if (benificiary != null)
                return Request.CreateResponse(HttpStatusCode.OK, benificiary);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Beneficiary found.");
        }

        [HttpGet]
        [Route("Validate/{userName}/{password}")]
        public HttpResponseMessage ValidatedUser(int userName,string password)
        {
            var benificiary = _benificiaryService.AuthenticateBeneficiary(userName, password);
            if (benificiary != null)
            {
                BeneficiaryAPIViewModel beneficiaryAPIViewModel = new BeneficiaryAPIViewModel();
                beneficiaryAPIViewModel.BeneficiaryId = benificiary.BeneficiaryId;
                beneficiaryAPIViewModel.BeneficiaryName = benificiary.BeneficairyName;
                beneficiaryAPIViewModel.AdharNo = Convert.ToInt32(benificiary.AdharNo);
                beneficiaryAPIViewModel.Address = benificiary.PresentAddress;
                beneficiaryAPIViewModel.GeoTaggingDetails = benificiary.GeoTaggingDetails.Select(s => new GeoTaggingViewModel
                {
                    BeneficiaryId = s.BeneficiaryId,
                    Date = s.Date,
                    Photo = "",//s.Photo,
                    InstallmentId = s.InstallmentId,
                    ConstructionLevel = s.ConstructionLevel
                }).ToList();
                beneficiaryAPIViewModel.Comments = benificiary.Comments.Select(s => new CommentsViewModel
                {
                    Comments = s.Comments,
                    InstallementId = s.InstallementId,
                    Reason = s.Reason,
                    RoleId = s.RoleId
                }).ToList();

                beneficiaryAPIViewModel.InstallmentDetails = benificiary.InstallmentDetails.Select(s => new InstallmentDetailsViewModel
                {
                    BeneficiaryAmnt = s.BeneficiaryAmnt,
                    CompanyID = s.CompanyID,
                    InstallmentId = s.InstallmentId,
                    InstallmentNo = s.InstallmentNo,
                    InstallmentSignings = s.InstallmentSignings.Select(i => new InstallmentSigningViewModel { RoleId = i.RoleId, Sign = i.Sign, CreatedDate = i.CreatedDate }).ToList()
                }).ToList();
                beneficiaryAPIViewModel.InstallmentNo = Convert.ToInt32(benificiary.InstallmentDetails.OrderByDescending(o => o.InstallmentId).Select(s => s.InstallmentNo).FirstOrDefault());
                if (benificiary != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, beneficiaryAPIViewModel);
                }

                else
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden, "You are not having access application, Please contact administrator.");
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "User does not exist.");
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
