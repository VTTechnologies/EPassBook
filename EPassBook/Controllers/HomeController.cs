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
    //[AuthorizeAttribute]
    public class HomeController : Controller
    {
        IUserService _userserService;
        ICityService _cityMasterService;
        IRoleMasterService _roleMasterService;
        ICompanyMasterService _companyMasterService;


        public HomeController(IUserService userserService, ICityService cityMasterService,
            IRoleMasterService roleMasterService, ICompanyMasterService companyMasterService)
        {
            _companyMasterService = companyMasterService;
            _roleMasterService = roleMasterService;
            _cityMasterService = cityMasterService;
            _userserService = userserService;
        }

        //added by ather
        public ActionResult Website()
        {
            return View();
        }

        public ActionResult Website2()
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

        [HttpGet]
        public ActionResult SendMessage(string name, string email, string subject, string message)
        {
            
            GMailer.GmailUsername = "";
            GMailer.GmailPassword = "";

            GMailer mailer = new GMailer();
            mailer.ToEmail = "";
            mailer.Body = "Name: " + name + "   Email: " + email + "  Message: " + message;
            mailer.Subject = subject;
            mailer.IsHtml = true;
            mailer.Send();

            return View();
        }
    }     
}