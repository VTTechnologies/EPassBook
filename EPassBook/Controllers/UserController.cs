using AutoMapper;
using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPassBook.Controllers
{
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
    }
}