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
        ICommentService _icommentService;

        public UserController(IUserService userService, IMapper mapper,ICommentService commentService)
        {
            _icommentService = commentService;
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
            _userService.Add(userModel);
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
        public ActionResult Login([Bind(Include = "UserName,Password")]UserViewModel user)
        {
            if (ModelState.IsValidField("UserName")&& ModelState.IsValidField("Password"))
            {
                var userData = _userService.GetPassword(user.UserName);
                if(user.RememberMe)
                {
                    HttpCookie userCookie = new HttpCookie("userinfo");
                    userCookie["user"] = user.UserName;
                    userCookie["password"] = user.Password;
                    userCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(userCookie);
                }

                user = _mapper.Map<UserMaster, UserViewModel>(userData);
                Session["UserDetails"] = user;
                //var userData = _userService.AuthenticateUser(user.UserName, user.Password);
                if (userData != null)
                {
                    if(user.Password.Equals(userData.Password))
                    {
                        Session["CompID"] = userData.CompanyID;
                        return RedirectToAction("Index", "WorkFlow");
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

        //ather code start frm here
        [HttpGet]
        public ActionResult SurveyDetails()
        {
            try
            {
                IEnumerable<sp_GetSurveyDetailsByBenID_Result> commentlist = _icommentService.GetSurveyDetailsByBenificiaryID(1);
                
                var mappedCommentList = _mapper.Map<IEnumerable<sp_GetSurveyDetailsByBenID_Result>, IEnumerable<SurveyDetailsModel>>(commentlist);
                
                return View(mappedCommentList);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}