using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPassBook.DAL.IService;
using EPassBook.Helper;

namespace EPassBook.Controllers
{
    [ElmahError]
    public class HomeController : Controller
    {
        IUserService _userser;
        
        public HomeController(IUserService userser)
        {
            _userser = userser;
        }

        [CustomAuthorize(Common.Admin)]
        public ActionResult Index()
        {
          
            return View();
        }
        //added by ather
        public ActionResult Website()
        {

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
    }
}