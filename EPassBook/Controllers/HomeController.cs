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
    [AuthorizeAttribute]
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