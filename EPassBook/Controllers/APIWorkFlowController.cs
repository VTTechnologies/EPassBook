using EPassBook.DAL.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static EPassBook.Helper.Common;

namespace EPassBook.Controllers
{
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
        public HttpResponseMessage ValidatedUser(int userName,string password)
        {
            var benificiary = _benificiaryService.AuthenticateBeneficiary(userName, password);
            if (benificiary!=null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, benificiary);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden,"You are not having access application, Please contact administrator.");
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateInstallmentStatus(int beneficiaryId,int installmentNo)
        {
            var installmentDetail = _installmentDetailService.GetAllInstallmentDetails().Where(w=>w.BeneficiaryId==beneficiaryId && w.InstallmentNo== installmentNo ).FirstOrDefault();
            installmentDetail.StageID = Convert.ToInt32(WorkFlowStages.UserRequest);
            installmentDetail.ModifiedBy = "Beneficiary";
            installmentDetail.ModifiedDate = DateTime.Now;

            if (installmentDetail != null)
                return Request.CreateResponse(HttpStatusCode.OK, installmentDetail);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No product found for this id");
        }
    }
}
