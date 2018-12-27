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
    [ElmahError]
    public class WorkFlowController : Controller
    {
        IInstallmentDetailService _installmentDetailService;
        IBenificiaryService _benificiaryService;
        IWorkFlowStagesService _iWorkFlowStagesService;
        ICommentService _icommentService;


        public WorkFlowController(IInstallmentDetailService installmentDetailService, IBenificiaryService benificiaryService, IWorkFlowStagesService iWorkFlowStagesService, ICommentService icommentService)
        {
            _iWorkFlowStagesService = iWorkFlowStagesService;
            _benificiaryService = benificiaryService;
            _installmentDetailService = installmentDetailService;
            _icommentService = icommentService;
        }
        // GET: WorkFlow
        [HttpGet]
        [CustomAuthorize(Common.Admin, Common.SiteEngineer, Common.Accountant, Common.ChiefOfficer, Common.CityEngineer, Common.ProjectEngineer)]
        public ActionResult Index()
        {
            List<int> stageIds;
            string strStageIds = "";
            if (Session["UserDetails"] != null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                var roleId = user.UserInRoles.Select(s => s.RoleId).ToList();
                stageIds = _iWorkFlowStagesService.GetWorkflowStageById(roleId).ToList();
                strStageIds = string.Join(",", stageIds.ToArray());
            }
            var installmentListView = _installmentDetailService.GetInstallmentForLoginUsersWithStages(strStageIds).ToList();
            var resultlist = installmentListView.Select(s => new InstallmentListView
            {
                BeneficiaryId = s.BeneficiaryId,
                BeneficairyName = s.BeneficairyName,
                CompanyID = s.CompanyID,
                InstallmentId = s.InstallmentId,
                InstallmentNo = s.InstallmentNo,
                CreatedDate = s.CreatedDate,
                IsCompleted = s.IsCompleted,
                MobileNo = Convert.ToString(s.MobileNo),
                PlanYear = s.PlanYear,
                StageID = s.StageID
            }).ToList();
            return View(resultlist);
        }

        [HttpGet]
        [CustomAuthorize(Common.Admin, Common.SiteEngineer, Common.Accountant, Common.ChiefOfficer, Common.CityEngineer, Common.ProjectEngineer)]
        public ActionResult Workflow(int? id)
        {
            var benificiary = _benificiaryService.GetBenificiaryById(1);
            var benficiarymodel = Mapper.BeneficiaryMapper.Detach(benificiary);
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
        [CustomAuthorize(Common.Admin, Common.SiteEngineer, Common.Accountant, Common.ChiefOfficer, Common.CityEngineer, Common.ProjectEngineer)]
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
        [CustomAuthorize(Common.Accountant)]
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
                installmentDetail.StageID = (int)Common.WorkFlowStages.LastChiefOfficer;

                if (ModelState.IsValid)
                {
                    installmentDetail.InstallmentSignings.Add(instSigning);
                    _installmentDetailService.Update(installmentDetail);
                    _installmentDetailService.SaveChanges();
                    ViewBag.Message = "sussess message";
                    return View("_Accountant", accountDetailsVM);
                }
                else
                {
                    var benificiaryDetails = _benificiaryService.GetBenificiaryById(installmentDetail.BeneficiaryId);
                    accountDetailsVM.InstallmentId = Convert.ToInt32(accountDetailsVM.InstallmentId);
                    accountDetailsVM.LoanAmnt = Convert.ToInt32(installmentDetail.LoanAmnt);
                    accountDetailsVM.IFSCCode = benificiaryDetails.IFSCCode;
                    accountDetailsVM.AccountNo = benificiaryDetails.AccountNo.ToString();
                    accountDetailsVM.LoanAmtInRupees = accountDetailsVM.LoanAmnt.ConvertNumbertoWords();
                    
                    return View("_Accountant", accountDetailsVM);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet]
        [CustomAuthorize(Common.Admin, Common.SiteEngineer, Common.Accountant, Common.ChiefOfficer, Common.CityEngineer, Common.ProjectEngineer)]
        public ActionResult SiteEngineer()
        {
            int installmentid = 0;
            if (Session["InstallmentId"] != null)
            {
                installmentid = Convert.ToInt32(Session["InstallmentId"]);
            }
            InstallmentDetailsViewModel installvm = new InstallmentDetailsViewModel();
            var installment = _installmentDetailService.GetInstallmentDetailById(installmentid);
            var installmentviewmodel = Mapper.InstallmentDetailsMapper.Detach(installment);
            installmentviewmodel.Comments = null;
            return PartialView("_SiteEngineer", installmentviewmodel);
        }

        [HttpPost]
        [CustomAuthorize(Common.SiteEngineer)]
        public ActionResult SiteEngineer(InstallmentDetailsViewModel installmentDetailViewModel, string IsRadioButton)
        {
            HttpPostedFileBase hasbandphoto = Request.Files["imguploadsiteeng"];
            var installment = _installmentDetailService.GetInstallmentDetailById(installmentDetailViewModel.InstallmentId);
            if (ModelState.IsValid)
            {
                if (Session["UserDetails"] != null)
                {
                    bool iscenter = true;

                    if (IsRadioButton == "State Assistance")
                    {
                        iscenter = false;
                    }

                    var user = Session["UserDetails"] as UserViewModel;
                    installment.ModifiedBy = user.UserName;
                    installment.CompanyID = user.CompanyID;

                    installment.BeneficiaryAmnt = installmentDetailViewModel.BeneficiaryAmnt;
                    installment.LoanAmnt = installmentDetailViewModel.LoanAmnt;
                    installment.IsCentreAmnt = iscenter;
                    installment.ConstructionLevel = installmentDetailViewModel.ConstructionLevel;
                    installment.StageID = (int)Common.WorkFlowStages.ProjectEngineer;
                    installment.InstallmentNo = installmentDetailViewModel.InstallmentNo;
                    installment.ModifiedDate = DateTime.Now;

                    // Insert reocrd in comment table 
                    var comments = new Comment();
                    comments.Comments = installmentDetailViewModel._Comments;
                    comments.CreatedBy = user.UserName;
                    comments.BeneficiaryId = installmentDetailViewModel.BeneficiaryId;
                    comments.CreatedDate = DateTime.Now;
                    comments.CompanyID = user.CompanyID;

                    // Insert reocrd in GeoTaggingDetail table 
                    var geotaging = new GeoTaggingDetail();
                    geotaging.BeneficiaryId = installmentDetailViewModel.BeneficiaryId;
                    geotaging.CompanyID = user.CompanyID;
                    geotaging.ConstructionLevel = installmentDetailViewModel.ConstructionLevel;
                    geotaging.UserId = user.UserId;
                    geotaging.CreatedBy = user.UserName;
                    geotaging.CreatedDate = DateTime.Now;
                    geotaging.Photo = "";//pm.ConvertToBytes(hasbandphoto);

                    // Insert reocrd in GeoTaggingDetail table 
                    var signing = new InstallmentSigning();
                    signing.InstallmentId = installmentDetailViewModel.InstallmentId;
                    signing.UserId = user.UserId;
                    signing.RoleId = user.UserInRoles.FirstOrDefault().RoleId;
                    signing.Sign = true;
                    signing.CreatedDate = DateTime.Now;
                    signing.CompanyID = user.CompanyID;

                    // Applying changes to database tables
                    installment.Comments.Add(comments);
                    installment.GeoTaggingDetails.Add(geotaging);
                    installment.InstallmentSignings.Add(signing);
                    _installmentDetailService.Update(installment);


                    _installmentDetailService.SaveChanges();

                    Session["InstallmentId"] = null;
                    ViewBag.Message = "sussess message";
                    //return RedirectToAction("Index", "WorkFlow");
                }
            }

            return PartialView("_SiteEngineer", installmentDetailViewModel);
        }

        [HttpGet]
        [CustomAuthorize(Common.Admin, Common.SiteEngineer, Common.Accountant, Common.ChiefOfficer, Common.CityEngineer, Common.ProjectEngineer)]
        public ActionResult SurveyDetails()
        {
            try
            {
                IEnumerable<sp_GetSurveyDetailsByBenID_Result> commentlist = _icommentService.GetSurveyDetailsByBenificiaryID(1);

                var mappedCommentList = commentlist.Select(s => new SurveyDetailsModel
                {
                    BeneficiaryId = s.BeneficiaryId,
                    Comments = s.Comments,
                    UserName = s.UserName,
                    CreatedDate = s.CreatedDate,
                    MobileNo = s.MobileNo,
                    Sign = s.Sign,
                    Physical_Progress = s.Physical_Progress

                }).ToList();

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        [CustomAuthorize(Common.Admin, Common.SiteEngineer, Common.Accountant, Common.ChiefOfficer, Common.CityEngineer, Common.ProjectEngineer)]
        public ActionResult DataEntry()
        {
            return PartialView("_DataEntry", new BeneficiaryViewModel());
        }

        [HttpPost]
        [CustomAuthorize(Common.DataEntry)]
        public ActionResult DataEntry(BeneficiaryViewModel beneficiaryViewModel)
        {
            HttpPostedFileBase hasbandphoto = Request.Files["imgupload1"];
            HttpPostedFileBase wifephoto = Request.Files["imgupload2"];
            var user = Session["UserDetails"] as UserViewModel;
            beneficiaryViewModel.Hasband_Photo = ""; //PhotoManager.ConvertToBytes(hasbandphoto);
            beneficiaryViewModel.Wife_Photo = ""; PhotoManager.ConvertToBytes(wifephoto);
            beneficiaryViewModel.CreatedBy = user.UserName;
            var insertbeneficiary = Mapper.BeneficiaryMapper.Attach(beneficiaryViewModel);
            _benificiaryService.Add(insertbeneficiary);
            _benificiaryService.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [CustomAuthorize(Common.Admin, Common.SiteEngineer, Common.Accountant, Common.ChiefOfficer, Common.CityEngineer, Common.ProjectEngineer)]
        public ActionResult ProjectEngineer()
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
            installmentviewmodel._Comments = null;
            return PartialView("_ProjectEngineer", installmentviewmodel);
        }

        [HttpPost]
        [CustomAuthorize(Common.ProjectEngineer)]
        public ActionResult ProjectEngineer(InstallmentDetailsViewModel installmentDetailViewModel)
        {

            var installment = _installmentDetailService.GetInstallmentDetailById(installmentDetailViewModel.InstallmentId);
            //if (ModelState.IsValid)
            //{
            if (Session["UserDetails"] != null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                installment.ModifiedBy = user.UserName;
                installment.StageID = (int)Common.WorkFlowStages.CityEngineer;
                installment.ModifiedDate = DateTime.Now;

                // Insert reocrd in comment table 
                var comments = new Comment();
                comments.Comments = installmentDetailViewModel._Comments;
                comments.CreatedBy = user.UserName;
                comments.BeneficiaryId = installmentDetailViewModel.BeneficiaryId;
                comments.CreatedDate = DateTime.Now;
                comments.CompanyID = user.CompanyID;

                // Insert reocrd in GeoTaggingDetail table 
                var signing = new InstallmentSigning();
                signing.InstallmentId = installmentDetailViewModel.InstallmentId;
                signing.UserId = user.UserId;
                signing.RoleId = user.UserInRoles.FirstOrDefault().RoleId;
                signing.Sign = true;
                signing.CreatedDate = DateTime.Now;
                signing.CompanyID = user.CompanyID;

                // Applying changes to database tables
                installment.Comments.Add(comments);

                installment.InstallmentSignings.Add(signing);
                _installmentDetailService.Update(installment);


                _installmentDetailService.SaveChanges();

                Session["InstallmentId"] = null;
                ViewBag.Message = "sussess message";

                installmentDetailViewModel.BeneficiaryAmnt = installment.BeneficiaryAmnt;
                installmentDetailViewModel.LoanAmnt = installment.LoanAmnt;
                installmentDetailViewModel.ConstructionLevel = installment.ConstructionLevel;

                //return RedirectToAction("Index", "WorkFlow");
            }
            //}

            return PartialView("_ProjectEngineer", installmentDetailViewModel);
        }

        [HttpGet]
        [CustomAuthorize(Common.Admin, Common.SiteEngineer, Common.Accountant, Common.ChiefOfficer, Common.CityEngineer, Common.ProjectEngineer)]
        public ActionResult CityEngineer()
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
            installmentviewmodel._Comments = null;
            return PartialView("_CityEngineer", installmentviewmodel);
        }

        [HttpPost]
        [CustomAuthorize(Common.CityEngineer)]
        public ActionResult CityEngineer(InstallmentDetailsViewModel installmentDetailViewModel)
        {

            var installment = _installmentDetailService.GetInstallmentDetailById(installmentDetailViewModel.InstallmentId);
            //if (ModelState.IsValid)
            //{
            if (Session["UserDetails"] != null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                installment.ModifiedBy = user.UserName;
                installment.StageID = (int)Common.WorkFlowStages.ChiefOfficer;
                installment.ModifiedDate = DateTime.Now;

                // Insert reocrd in comment table 
                var comments = new Comment();
                comments.Comments = installmentDetailViewModel._Comments;
                comments.CreatedBy = user.UserName;
                comments.BeneficiaryId = installmentDetailViewModel.BeneficiaryId;
                comments.CreatedDate = DateTime.Now;
                comments.CompanyID = user.CompanyID;

                // Insert reocrd in GeoTaggingDetail table 
                var signing = new InstallmentSigning();
                signing.InstallmentId = installmentDetailViewModel.InstallmentId;
                signing.UserId = user.UserId;
                signing.RoleId = user.UserInRoles.FirstOrDefault().RoleId;
                signing.Sign = true;
                signing.CreatedDate = DateTime.Now;
                signing.CompanyID = user.CompanyID;

                // Applying changes to database tables
                installment.Comments.Add(comments);

                installment.InstallmentSignings.Add(signing);
                _installmentDetailService.Update(installment);


                _installmentDetailService.SaveChanges();

                Session["InstallmentId"] = null;
                ViewBag.Message = "sussess message";

                installmentDetailViewModel.BeneficiaryAmnt = installment.BeneficiaryAmnt;
                installmentDetailViewModel.LoanAmnt = installment.LoanAmnt;
                installmentDetailViewModel.ConstructionLevel = installment.ConstructionLevel;

                //return RedirectToAction("Index", "WorkFlow");
            }
            //}

            return PartialView("_CityEngineer", installmentDetailViewModel);
        }

        [HttpGet]
        [CustomAuthorize(Common.Admin, Common.SiteEngineer, Common.Accountant, Common.ChiefOfficer, Common.CityEngineer, Common.ProjectEngineer)]
        public ActionResult ChiefOfficer()
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
            installmentviewmodel._Comments = null;
            return PartialView("_ChiefOfficer", installmentviewmodel);
        }

        [HttpPost]
        [CustomAuthorize(Common.ChiefOfficer)]
        public ActionResult ChiefOfficer(InstallmentDetailsViewModel installmentDetailViewModel)
        {

            var installment = _installmentDetailService.GetInstallmentDetailById(installmentDetailViewModel.InstallmentId);
            //if (ModelState.IsValid)
            //{
            if (Session["UserDetails"] != null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                installment.ModifiedBy = user.UserName;
                installment.StageID = (int)Common.WorkFlowStages.Accountant;
                installment.ModifiedDate = DateTime.Now;

                // Insert reocrd in comment table 
                var comments = new Comment();
                comments.Comments = installmentDetailViewModel._Comments;
                comments.CreatedBy = user.UserName;
                comments.BeneficiaryId = installmentDetailViewModel.BeneficiaryId;
                comments.CreatedDate = DateTime.Now;
                comments.CompanyID = user.CompanyID;

                // Insert reocrd in GeoTaggingDetail table 
                var signing = new InstallmentSigning();
                signing.InstallmentId = installmentDetailViewModel.InstallmentId;
                signing.UserId = user.UserId;
                signing.RoleId = user.UserInRoles.FirstOrDefault().RoleId;
                signing.Sign = true;
                signing.CreatedDate = DateTime.Now;
                signing.CompanyID = user.CompanyID;

                // Applying changes to database tables
                installment.Comments.Add(comments);

                installment.InstallmentSignings.Add(signing);
                _installmentDetailService.Update(installment);


                _installmentDetailService.SaveChanges();

                Session["InstallmentId"] = null;
                ViewBag.Message = "sussess message";

                installmentDetailViewModel.BeneficiaryAmnt = installment.BeneficiaryAmnt;
                installmentDetailViewModel.LoanAmnt = installment.LoanAmnt;
                installmentDetailViewModel.ConstructionLevel = installment.ConstructionLevel;

                //return RedirectToAction("Index", "WorkFlow");
            }
            //}

            return PartialView("_ChiefOfficer", installmentDetailViewModel);
        }
    }
}