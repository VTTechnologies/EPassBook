﻿using AutoMapper;
using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPassBook.Helper;

namespace EPassBook.Controllers
{
    public class WorkFlowController : Controller
    {
        private readonly IMapper _mapper;
        IInstallmentDetailService _installmentDetailService;
        IBenificiary _benificiaryService;

        public WorkFlowController(IMapper mapper, IInstallmentDetailService installmentDetailService, IBenificiary benificiaryService)
        {
            _benificiaryService = benificiaryService;
            _installmentDetailService = installmentDetailService;
            _mapper = mapper;
        }
        // GET: WorkFlow
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateAccountant(int id)
        {
            Session["BeniId"] = id;
            AccountDetailsViewModel advm = new AccountDetailsViewModel();
            InstallmentSigning instS = new InstallmentSigning();
            var installmentDetails = _installmentDetailService.GetInstallmentDetailById(installmentId);
            var benificiaryDetails = _benificiaryService.GetBenificiaryById(installmentDetails.BeneficiaryId);

            advm.LoanAmnt = Convert.ToInt32(installmentDetails.LoanAmnt);
            advm.IFSCCode = benificiaryDetails.IFSCCode;
            advm.AccountNo = benificiaryDetails.AccountNo.ToString();
            advm.LoanAmtInRupees = advm.LoanAmnt.ConvertNumbertoWords();
            return View(advm);
        }

        [HttpPost]
        public ActionResult CreateAccountant(AccountDetailsViewModel accountDetailsVM)
        {
            if(Session["UserDetails"] !=null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                int beniId = 0;
                if (Session["BeniId"] != null)
                {
                    beniId = Convert.ToInt32(Session["BeniId"]);
                }
                else
                {
                    RedirectToAction("Login", "User");
                }
                var installmentDetail = _installmentDetailService.GetInstallmentDetailById(beniId); //id pass just for testing purpose
                var instSigning = new InstallmentSigning();
                UserInRole uir = new UserInRole();

                instSigning.InstallmentId = accountDetailsVM.InstallmentId;
                instSigning.Sign = accountDetailsVM.Sign;
                instSigning.UserId = user.UserId;
                instSigning.RoleId = user.UserInRoles.FirstOrDefault().RoleId;
                instSigning.CreatedDate = DateTime.Now;
                instSigning.CreatedBy = user.UserName;
                instSigning.CompanyID = user.CompanyID;

                installmentDetail.InstallmentSignings.Add(instSigning);

                installmentDetail.TransactionID = Convert.ToDecimal(accountDetailsVM.TransactionId);
                installmentDetail.ModifiedBy = user.UserName;
                installmentDetail.ModifiedDate = DateTime.Now;

                _installmentDetailService.Update(installmentDetail);
                _installmentDetailService.SaveChanges();
            }
            else
            {
                RedirectToAction("Login", "User");
            }
            return View();
        }
    }
}