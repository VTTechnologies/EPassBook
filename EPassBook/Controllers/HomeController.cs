﻿using AutoMapper;
using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using System;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

namespace EPassBook.Controllers
{
    [ElmahError]
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        IUserService _userser;
        IBenificiary _Ibenificiary;
        IInstallmentDetailService _installmentDetailService;
        ICityService _cityMasterService;
        IRoleMasterService _roleMasterService;
        ICompanyMasterService _companyMasterService;
        PhotoManager pm = new PhotoManager();

        public HomeController(IUserService userser, IBenificiary Ibenificiary, IMapper mapper, IInstallmentDetailService installmentDetailService,ICityService cityMasterService,
            IRoleMasterService roleMasterService, ICompanyMasterService companyMasterService)
        {
            _companyMasterService = companyMasterService;
            _roleMasterService = roleMasterService;
            _cityMasterService = cityMasterService;
            _userser = userser;
            _Ibenificiary = Ibenificiary;
            _mapper = mapper;
            _installmentDetailService = installmentDetailService;
        }

        [CustomAuthorize( Common.Admin, Common.SiteEngineer)]
        public ActionResult Index(int? id)
        {
            var benificiary = _Ibenificiary.GetBenificiaryById(1);
            var benficiarymodel = _mapper.Map<BenificiaryMaster, BeneficiaryViewModel>(benificiary);
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

        //added by ather
        public ActionResult Website()
        {

            return View();
        }
        [CustomAuthorize(Common.Admin)]
        [HttpGet]
        public ActionResult AddUser()
        {
            //added all Roles in list
            List<RoleMaster> roleList = _roleMasterService.GetAllRoles().ToList();
            //added all cities in list
            List<CityMaster> cityList = _cityMasterService.GetAllCities().ToList();
            //added all companies in list
            List<CompanyMaster> companyList = _companyMasterService.GetAllCompanies().ToList();


            // passed RoleList to viewdata for binding to Role dropdown
            TempData["roles"] = new SelectList(roleList, "RoleId", "RoleName");
            // passed City List to viewdata for binding to city dropdown
            TempData["Cities"] = new SelectList(cityList, "CityId", "CityName");
            // passed Company list to viewdata for binding to Role dropdown
            //ViewData["companies"] = new SelectList(companyList, "CompanyID", "CompanyName");
            TempData["companies"] = new SelectList(companyList, "CompanyID", "CompanyName");
            TempData.Keep();
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(UserViewModel userVM)
        {
            var userData = _mapper.Map<UserViewModel, UserMaster>(userVM);
            
            var userInRole = new UserInRoleViewModel();

            _userser.Add(userData);
            _userser.SaveChanges();
            int id = userVM.UserId;

            userInRole.UserId = id;
            userInRole.RoleId = userVM.RoleId;
            var roleData = _mapper.Map<UserInRoleViewModel, UserInRole>(userInRole);
            userData.UserInRoles.Add(roleData);
            _userser.SaveChanges();

            return View();
        }
        //end ather code

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Beneficiary()
        {
            return PartialView("Beneficiary", new BeneficiaryViewModel());
        }
        [Route("Beneficiary")]
        [HttpPost]
        public ActionResult Beneficiary(BeneficiaryViewModel BVM)
        {
            HttpPostedFileBase hasbandphoto = Request.Files["imgupload1"];
            HttpPostedFileBase wifephoto = Request.Files["imgupload2"];
            int i = UploadImageInDataBase(hasbandphoto, wifephoto, BVM);
            if (i==1)
            {
                return RedirectToAction("Index");
            }
            return View(BVM);
            
           
        }
       // private readonly DBContext db = new DBContext();
        public int UploadImageInDataBase(HttpPostedFileBase hphoto,HttpPostedFileBase wphoto, BeneficiaryViewModel BVM)
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
                _Ibenificiary.Add(insertbeneficiary);
                _Ibenificiary.SaveChanges();
                return 1;
            
        }
       


        //public ActionResult _InstallmentDetails()
        //{
        //    var benfici = _Ibenificiary.GetBenificiaryById(1);            
        //    var benficiarymodel = _mapper.Map<BenificiaryMaster, BeneficiaryViewModel>(benfici);
        //    DateTime currentdate = Convert.ToDateTime(benficiarymodel.CreatedDate);            

        //    return PartialView(benficiarymodel);
        //}

        [HttpGet]
        [ChildActionOnly]
        public ActionResult _CityHead()
        {
           
            int installmentid = 0;
            if (Session["InstallmentId"] != null)
            {
                installmentid = Convert.ToInt32(Session["InstallmentId"]);
            }
            InstallmentDetailsViewModel installvm = new InstallmentDetailsViewModel();
            var installment = _installmentDetailService.GetInstallmentDetailById(installmentid);
            var installmentviewmodel = _mapper.Map<InstallmentDetail, InstallmentDetailsViewModel>(installment);
            installmentviewmodel._Comments = "";
            return PartialView(installmentviewmodel);
        }

        [HttpPost]
        
        public ActionResult _CityHead(InstallmentDetailsViewModel installmentDetailViewModel)
        {
            HttpPostedFileBase hasbandphoto = Request.Files["imguploadsiteeng"];
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
                installment.StageID = (int)Common.WorkFlowStages.ProjectManager;
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
                geotaging.Photo = pm.ConvertToBytes(hasbandphoto);

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
            return View();
        }

    }
}