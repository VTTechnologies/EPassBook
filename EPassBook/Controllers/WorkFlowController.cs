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
        IInstallmentDetailService _installmentDetailService;
        IBenificiaryService _benificiaryService;
        IWorkFlowStagesService _iWorkFlowStagesService;
        ICommentService _icommentService;

        PhotoManager pm = new PhotoManager();
        public WorkFlowController(IInstallmentDetailService installmentDetailService, IBenificiaryService benificiaryService, IWorkFlowStagesService iWorkFlowStagesService, ICommentService icommentService)
        {
            _iWorkFlowStagesService = iWorkFlowStagesService;
            _benificiaryService = benificiaryService;
            _installmentDetailService = installmentDetailService;
            _icommentService= icommentService;
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
            var resultlist = installmentListView.Select(s => new InstallmentListView
            {
                BeneficiaryId = s.BeneficiaryId,
                BeneficairyName=s.BeneficairyName,
                CompanyID=s.CompanyID,
                InstallmentId=s.InstallmentId,
                InstallmentNo=s.InstallmentNo,
                CreatedDate=Convert.ToDateTime(s.CreatedDate),
                IsCompleted=s.IsCompleted,
                MobileNo=Convert.ToString(s.MobileNo),
                PlanYear=s.PlanYear,
                StageID=s.StageID
            }).ToList();  //_mapper.Map<IEnumerable<sp_GetInstallmentListViewForUsersRoles_Result>, IEnumerable<InstallmentListView>>(installmentListView);
            return View();
        }


        [CustomAuthorize(Common.Admin, Common.SiteEngineer)]
        public ActionResult Workflow(int? id)
        {
            var benificiary = _benificiaryService.GetBenificiaryById(1);
            var benficiarymodel = Mapper.BeneficiaryMapper.Detach(benificiary);  //_mapper.Map<BeneficiaryViewModel>(benificiary);
            Session["InstallmentId"] = id;
            string rolename = "";
            if (Session["UserDetails"] != null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                var roleId = user.UserInRoles.Select(s => s.RoleId).FirstOrDefault();
                rolename = Enum.GetName(typeof(Common.WorkFlowStages), roleId);
            }
            ViewBag.RoleName = rolename;
            return View(benficiarymodel);
        }

        [HttpGet]
        public ActionResult Accountant(int id)
        {
            AccountDetailsViewModel accountDetailsViewModel = new AccountDetailsViewModel();
            InstallmentSigning instS = new InstallmentSigning();
            var installmentDetails = _installmentDetailService.GetInstallmentDetailById(id);
            var benificiaryDetails = _benificiaryService.GetBenificiaryById(installmentDetails.BeneficiaryId);
            accountDetailsViewModel.InstallmentId = id;

            accountDetailsViewModel.LoanAmnt = Convert.ToInt32(installmentDetails.LoanAmnt);
            accountDetailsViewModel.IFSCCode = benificiaryDetails.IFSCCode;
            accountDetailsViewModel.AccountNo = benificiaryDetails.AccountNo.ToString();
            accountDetailsViewModel.LoanAmtInRupees = accountDetailsViewModel.LoanAmnt.ConvertNumbertoWords();
            return PartialView("_Accountant", accountDetailsViewModel);
        }

        [HttpPost]
        public ActionResult Accountant(AccountDetailsViewModel accountDetailsVM)
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
        [HttpGet]
        public ActionResult SiteEngineer()
        {
            int installmentid = 0;
            if (Session["InstallmentId"] != null)
            {
                installmentid = Convert.ToInt32(Session["InstallmentId"]);
            }
            InstallmentDetailsViewModel installvm = new InstallmentDetailsViewModel();
            var installment = _installmentDetailService.GetInstallmentDetailById(installmentid);
            var installmentviewmodel = Mapper.InstallmentDetailsMapper.Detach(installment);// _mapper.Map<InstallmentDetail, InstallmentDetailsViewModel>(installment);
            installmentviewmodel.Comments = null;
            return PartialView("_SiteEngineer", installmentviewmodel);
        }

        [HttpPost]
        public ActionResult SiteEngineer(InstallmentDetailsViewModel installmentDetailViewModel)
        {
            var installment = _installmentDetailService.GetInstallmentDetailById(installmentDetailViewModel.InstallmentId);

            if (Session["UserDetails"] != null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                installment.ModifiedBy = user.UserName;
                installment.CompanyID = user.CompanyID;

                installment.BeneficiaryAmnt = installmentDetailViewModel.BeneficiaryAmnt;
                installment.LoanAmnt = installmentDetailViewModel.LoanAmnt;
                installment.IsCentreAmnt = installmentDetailViewModel.IsCentreAmnt;
                installment.ConstructionLevel = installmentDetailViewModel.ConstructionLevel;
                installment.StageID = (int)Common.WorkFlowStages.ProjectEngineer;
                installment.InstallmentNo = installmentDetailViewModel.InstallmentNo;
                installment.ModifiedDate = DateTime.Now;

                var comments = new Comment();
                comments.Comments = installmentDetailViewModel._Comments;
                comments.CreatedBy = user.UserName;
                comments.BeneficiaryId = installmentDetailViewModel.BeneficiaryId;
                comments.CreatedDate = DateTime.Now;
                comments.CompanyID = user.CompanyID;

                installment.Comments.Add(comments);

                _installmentDetailService.Update(installment);
                _installmentDetailService.SaveChanges();

                Session["InstallmentId"] = null;
            }
            return View();
        }

        [HttpGet]
        public ActionResult SurveyDetails()
        {
            try
            {
                IEnumerable<sp_GetSurveyDetailsByBenID_Result> commentlist = _icommentService.GetSurveyDetailsByBenificiaryID(1);

                var mappedCommentList = commentlist.Select(s=> new SurveyDetailsModel {
                    BeneficiaryId =s.BeneficiaryId,
                    Comments =s.Comments,
                    UserName =s.UserName,
                    CreatedDate =s.CreatedDate,
                    MobileNo =s.MobileNo,
                    Sign =s.Sign,
                    Physical_Progress =s.Physical_Progress

                }).ToList();// _mapper.Map<IEnumerable<sp_GetSurveyDetailsByBenID_Result>, IEnumerable<SurveyDetailsModel>>(commentlist);

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult DataEntry()
        {
            return PartialView("_DataEntry", new BeneficiaryViewModel());
        }
        [HttpPost]
        public ActionResult DataEntry(BeneficiaryViewModel BVM)
        {
            HttpPostedFileBase hasbandphoto = Request.Files["imgupload1"];
            HttpPostedFileBase wifephoto = Request.Files["imgupload2"];
            int i = UploadImageInDataBase(hasbandphoto, wifephoto, BVM);
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return View(BVM);


        }
        [NonAction]
        public int UploadImageInDataBase(HttpPostedFileBase hphoto, HttpPostedFileBase wphoto, BeneficiaryViewModel BVM)
        {
            BVM.Hasband_Photo = pm.ConvertToBytes(hphoto);
            BVM.Wife_Photo = pm.ConvertToBytes(wphoto);
            var user = Session["UserDetails"] as UserViewModel;
            var insertbeneficiary = new BenificiaryMaster();
            //var benficiarymodel = _mapper.Map<BeneficiaryViewModel, BenificiaryMaster>(BVM);
            insertbeneficiary.Hasband_Photo = BVM.Hasband_Photo;
            insertbeneficiary.Wife_Photo = BVM.Wife_Photo;
            insertbeneficiary.BeneficairyName = BVM.BeneficairyName;
            insertbeneficiary.FatherName = BVM.FatherName;
            insertbeneficiary.Mother = BVM.Mother;
            insertbeneficiary.MobileNo = BVM.MobileNo;
            insertbeneficiary.CityId = BVM.CityId;
            insertbeneficiary.DTRNo = BVM.DTRNo;
            insertbeneficiary.RecordNo = BVM.RecordNo;
            insertbeneficiary.Class = BVM.Class;
            insertbeneficiary.General = BVM.General;
            insertbeneficiary.Single = BVM.Single;
            insertbeneficiary.Disabled = BVM.Disabled;
            insertbeneficiary.Password = BVM.Password;
            insertbeneficiary.AdharNo = BVM.AdharNo;
            insertbeneficiary.VoterID = BVM.VoterID;
            insertbeneficiary.Area = BVM.Area;
            insertbeneficiary.MojaNo = BVM.MojaNo;
            insertbeneficiary.KhataNo = BVM.KhataNo;
            insertbeneficiary.KhasraNo = BVM.KhasraNo;
            insertbeneficiary.PlotNo = BVM.PlotNo;
            insertbeneficiary.PoliceStation = BVM.PoliceStation;
            insertbeneficiary.WardNo = BVM.WardNo;
            insertbeneficiary.District = BVM.District;
            insertbeneficiary.BankName = BVM.BankName;
            insertbeneficiary.BranchName = BVM.BranchName;
            insertbeneficiary.IFSCCode = BVM.IFSCCode;
            insertbeneficiary.AccountNo = BVM.AccountNo;
            insertbeneficiary.CreatedBy = user.UserName;
            insertbeneficiary.CreatedDate = DateTime.Now;
            insertbeneficiary.CompanyID = user.CompanyID;
            _benificiaryService.Add(insertbeneficiary);
            _benificiaryService.SaveChanges();
            return 1;

        }
    }
}