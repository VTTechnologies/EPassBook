using AutoMapper;
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
        IUserService _userserService;
        ICityService _cityMasterService;
        IRoleMasterService _roleMasterService;
        ICompanyMasterService _companyMasterService;
        

        public HomeController(IUserService userserService, IMapper mapper, ICityService cityMasterService,
            IRoleMasterService roleMasterService, ICompanyMasterService companyMasterService)
        {
            _companyMasterService = companyMasterService;
            _roleMasterService = roleMasterService;
            _cityMasterService = cityMasterService;
            _userserService = userserService;
            _mapper = mapper;
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

            _userserService.Add(userData);
            _userserService.SaveChanges();
            int id = userVM.UserId;

            userInRole.UserId = id;
            userInRole.RoleId = userVM.RoleId;
            var roleData = _mapper.Map<UserInRoleViewModel, UserInRole>(userInRole);
            userData.UserInRoles.Add(roleData);
            _userserService.SaveChanges();

            return View();
        }
      
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

    }
}