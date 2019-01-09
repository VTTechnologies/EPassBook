using EPassBook.DAL.IService;
using EPassBook.Helper;
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
    [ElmahError]
    [RoutePrefix("api/Workflow")]
    public class APIWorkFlowController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IBenificiaryService _benificiaryService;
        private readonly IInstallmentDetailService _installmentDetailService;
        private readonly ICommentService _iCommentService;

        public APIWorkFlowController(IUserService userService, IBenificiaryService benificiaryService, IInstallmentDetailService installmentDetailService, ICommentService iCommentService)
        {
            _benificiaryService = benificiaryService;
            _userService = userService;
            _installmentDetailService = installmentDetailService;
            _iCommentService = iCommentService;
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
            var benificiary = _benificiaryService.AuthenticateBeneficiary(userName, password);
            if (benificiary != null)
            {
                if (benificiary.BeneficiaryId != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, benificiary.BeneficiaryId);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,"User Name or Password is not corroect.");
                }
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
                    UserId = s.UserId
                }).ToList();

                beneficiaryAPIViewModel.Comments = installmentDetail.Comments.Select(s => new CommentsViewModel
                {
                    CreatedDate = s.CreatedDate,
                    CreatedBy = s.CreatedBy,
                    Comments = s.Comments,
                    Reason = s.Reason,
                    RoleId = s.RoleId
                }).ToList();

                beneficiaryAPIViewModel.InstallmentSignings = installmentDetail.InstallmentSignings.Select(i => new InstallmentSigningViewModel
                {
                    RoleId = i.RoleId,
                    Sign = i.Sign,
                    CreatedBy = i.CreatedBy,
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


        [Route("UpdateStatus/{installmentId}/{installmentNo}"), HttpPost]
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

                return Request.CreateResponse(HttpStatusCode.OK, "Request send successfully.");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is some problem, Please contact administrator.");
            }
        }

        [Route("AddComment/{installmentId}/{comment}"), HttpPost]
        public HttpResponseMessage AddComment(int installmentId, string comment)
        {
            var installmentDetail = _installmentDetailService.GetInstallmentDetailById(installmentId);

            if (installmentDetail != null)
            {
                installmentDetail.Comments.Add(new DAL.DBModel.Comment()
                {
                    InstallementId = installmentDetail.InstallmentId,
                    BeneficiaryId = installmentDetail.BeneficiaryId,
                    Comments = comment,
                    RoleId = Convert.ToInt32(Roles.Beneficiary),
                    CompanyID = installmentDetail.CompanyID,
                    CreatedDate = DateTime.Now,
                    CreatedBy = installmentDetail.CreatedBy
                });

                _installmentDetailService.Update(installmentDetail);
                _installmentDetailService.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Request send successfully.");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is some problem, Please contact administrator.");
            }
        }
    }
}
