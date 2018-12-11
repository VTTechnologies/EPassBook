using EPassBook.DAL.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPassBook.Controllers
{
    public class UserController : Controller
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
    }
}