using AutoMapper;
using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EPassBook.Controllers
{
    [ElmahError]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        IUserService _userService;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        // GET: User
        public ActionResult Index()
        {
            var users = _userService.GetAllUsers();           
            var userModel = _mapper.Map< IEnumerable<UserMaster>, IEnumerable<UserViewModel>>(users);

            return View(userModel);
        }
        [HttpGet]
        public ActionResult Create()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserViewModel user)
        {
            if(!ModelState.IsValid)
            {
                return View(user);

            }
            var userModel = _mapper.Map<UserViewModel, UserMaster>(user);
            _userService.Insert(userModel);
            _userService.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Test()
        {
            throw new Exception();
        }

        [HttpPost]
        //[CustomAuthorize(Common.Admin)]
        public ActionResult Login(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userData = _userService.GetPassword(user.UserName);
                //var userData = _userService.AuthenticateUser(user.UserName, user.Password);
                if (userData != null)
                {
                    if(user.Password.Equals(userData.Password))
                    {
                        Session["CompID"] = userData.CompanyID;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The password provided is incorrect.");
                        return View(user);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The username or password provided is incorrect.");
                    return View(user);
                }
            }
            else
            {
                return View(user);
            }
        }
    }
}