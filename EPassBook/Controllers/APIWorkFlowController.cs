using AutoMapper;
using EPassBook.DAL.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EPassBook.Controllers
{
    public class APIWorkFlowController : ApiController
    {
        private readonly IMapper _mapper;
        IUserService _userService;
        IBenificiaryService _benificiaryService;
        IInstallmentDetailService _installmentDetailService;

        public APIWorkFlowController(IUserService userService, IBenificiaryService benificiaryService, IInstallmentDetailService installmentDetailService, IMapper mapper)
        {
            _benificiaryService = benificiaryService;
            _userService = userService;
            _installmentDetailService = installmentDetailService;
            _mapper = mapper;
        }

        // GET api/product
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

        // GET api/product/5
        public HttpResponseMessage Get(int beneficiaryId)
        {
            var benificiary = _benificiaryService.GetBenificiaryById(beneficiaryId);
            if (benificiary != null)
                return Request.CreateResponse(HttpStatusCode.OK, benificiary);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Beneficiary found.");
        }


        // GET api/product/5
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

        // GET api/product/5
        public HttpResponseMessage UpdateInstallmentStatus(int beneficiaryId,int installmentNo)
        {
            var benificiary = _installmentDetailService.GetInstallmentDetailById(beneficiaryId);
            if (benificiary != null)
                return Request.CreateResponse(HttpStatusCode.OK, benificiary);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No product found for this id");
        }
    }
}
