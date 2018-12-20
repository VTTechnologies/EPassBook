using AutoMapper;
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
        IWorkFlowStagesService _iWorkFlowStagesService;

        public WorkFlowController(IMapper mapper, IInstallmentDetailService installmentDetailService, IBenificiary benificiaryService, IWorkFlowStagesService iWorkFlowStagesService)
        {
            _iWorkFlowStagesService = iWorkFlowStagesService;
            _benificiaryService = benificiaryService;
            _installmentDetailService = installmentDetailService;
            _mapper = mapper;
        }
        // GET: WorkFlow
        public ActionResult Index()
        {
            int stageId = 0;
            if (Session["UserDetails"] != null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                var roleId = user.UserInRoles.Select(s => s.RoleId).FirstOrDefault();
                stageId = _iWorkFlowStagesService.GetUserStageByRoleID(roleId);
            }

            var installmentListView = _installmentDetailService.GetInstallmentForLoginUsersWithStages(stageId).ToList();
            //var benficiarymodel = _mapper.Map<BenificiaryMaster, BeneficiaryViewModel>(benfici);
            var resultlist = _mapper.Map<IEnumerable<sp_GetInstallmentListViewForUsersRoles_Result>, IEnumerable<InstallmentListView>>(installmentListView);
            return View(resultlist);
        }

        [HttpGet]
        public ActionResult CreateAccountant(int id)
        {
            AccountDetailsViewModel advm = new AccountDetailsViewModel();
            InstallmentSigning instS = new InstallmentSigning();
            var installmentDetails = _installmentDetailService.GetInstallmentDetailById(id);
            var benificiaryDetails = _benificiaryService.GetBenificiaryById(installmentDetails.BeneficiaryId);
            advm.InstallmentId = id;

            advm.LoanAmnt = Convert.ToInt32(installmentDetails.LoanAmnt);
            advm.IFSCCode = benificiaryDetails.IFSCCode;
            advm.AccountNo = benificiaryDetails.AccountNo.ToString();
            advm.LoanAmtInRupees = advm.LoanAmnt.ConvertNumbertoWords();
            return View(advm);
        }

        [HttpPost]
        public ActionResult CreateAccountant(AccountDetailsViewModel accountDetailsVM)
        {
            if (Session["UserDetails"] != null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                var instSigning = new InstallmentSigning();
                var installmentDetail = _installmentDetailService.GetInstallmentDetailById(accountDetailsVM.InstallmentId);

                instSigning.InstallmentId = accountDetailsVM.InstallmentId;
                instSigning.Sign = accountDetailsVM.Sign;
                instSigning.UserId = user.UserId;
                instSigning.RoleId = user.UserInRoles.FirstOrDefault().RoleId;
                instSigning.CreatedDate = DateTime.Now;
                instSigning.CreatedBy = user.UserName;
                instSigning.CompanyID = user.CompanyID;

                installmentDetail.TransactionID = Convert.ToDecimal(accountDetailsVM.TransactionId);
                installmentDetail.ModifiedBy = user.UserName;
                installmentDetail.ModifiedDate = DateTime.Now;

                if (ModelState.IsValid)
                {
                    installmentDetail.InstallmentSignings.Add(instSigning);
                    _installmentDetailService.Update(installmentDetail);
                    _installmentDetailService.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var benificiaryDetails = _benificiaryService.GetBenificiaryById(installmentDetail.BeneficiaryId);
                    accountDetailsVM.InstallmentId = Convert.ToInt32(accountDetailsVM.InstallmentId);

                    accountDetailsVM.LoanAmnt = Convert.ToInt32(installmentDetail.LoanAmnt);
                    accountDetailsVM.IFSCCode = benificiaryDetails.IFSCCode;
                    accountDetailsVM.AccountNo = benificiaryDetails.AccountNo.ToString();
                    accountDetailsVM.LoanAmtInRupees = accountDetailsVM.LoanAmnt.ConvertNumbertoWords();
                    return View(accountDetailsVM);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
    }
}