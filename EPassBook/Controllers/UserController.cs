﻿using EPassBook.DAL.DBModel;
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
        IUserService _userService;
        IRoleMasterService _roleMasterService;
        ICityService _cityMasterService;
        ICompanyMasterService _companyMasterService;
        IUserInRoleService _userInRoleService;

        public UserController(IUserService userService, ICityService cityMasterService,
            IRoleMasterService roleMasterService, ICompanyMasterService companyMasterService, IUserInRoleService userInRoleService)
        {
            _userInRoleService = userInRoleService;
            _companyMasterService = companyMasterService;
            _roleMasterService = roleMasterService;
            _cityMasterService = cityMasterService;
            _userService = userService;
        }
        // GET: User
        public ActionResult IndexOld()
        {
            var users = _userService.GetAllUsers();

            var userModel = users.Select(s => new UserViewModel { UserId = s.UserId, UserName = s.UserName, Password = s.Password, IsActive = s.IsActive }).ToList();
            //var userModel = _mapper.Map< IEnumerable<UserMaster>, IEnumerable<UserViewModel>>(users);
           

            return View(userModel);
        }

        [HttpGet]
        public ActionResult CreateOld()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateOld(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var userModel = Mapper.UserMapper.Attach(user);
            //var userModel = _mapper.Map<UserViewModel, UserMaster>(user);
            _userService.Add(userModel);
            _userService.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind(Include = "UserName,Password")]UserViewModel user)
        {
            if (ModelState.IsValidField("UserName") && ModelState.IsValidField("Password"))
            {
                var userData = _userService.GetPassword(user.UserName);

                if (user.RememberMe)
                {
                    HttpCookie userCookie = new HttpCookie("userinfo");
                    userCookie["user"] = user.UserName;
                    userCookie["password"] = user.Password;
                    userCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(userCookie);
                }
                user = Mapper.UserMapper.Detach(userData);
                Session["UserDetails"] = "";
                Session["UserDetails"] = user;
                 if (userData != null)
                {
                    if (user.Password.Equals(userData.Password))
                    {
                        Session["CompID"] = userData.CompanyID;
                        if (userData.IsReset != true)
                        {
                            return RedirectToAction("ResetPassword", "User");
                        }
                        else
                        {
                            if (user.UserInRoles.FirstOrDefault().RoleId == Convert.ToInt32(Common.Roles.DataEntry))
                            {
                                return RedirectToAction("Index", "Beneficiary");
                            }
                            else
                            {
                                return RedirectToAction("Index", "WorkFlow");
                            }
                        }
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
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPassVM)
        {
            try
            {
                if (Session["UserDetails"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        var userData = Session["UserDetails"] as UserViewModel;
                        var user = _userService.GetUserById(userData.UserId);
                        if (resetPassVM.oldPassword != userData.Password)
                        {
                            ModelState.AddModelError(string.Empty, "Wrong old password");
                            return View(resetPassVM);
                        }
                        user.Password = resetPassVM.newPassword;
                        user.IsReset = true;
                        _userService.Update(user);
                        _userService.SaveChanges();
                        Session["UserDetails"] = _userService.GetUserById(userData.UserId);
                        return RedirectToAction("Index", "WorkFlow");
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        [CustomAuthorize(Common.Admin)]
        [HttpGet]
        public ActionResult Create()
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
            TempData["companies"] = new SelectList(companyList, "CompanyID", "CompanyName");
            TempData.Keep();
            return View();
        }
        [HttpPost]
        public ActionResult Create(UserViewModel userVM)
        {
            var userData = Mapper.UserMapper.Attach(userVM);
            var userInRole = new UserInRoleViewModel();
            _userService.Add(userData);
            _userService.SaveChanges();
            int id = userVM.UserId;
            userInRole.UserId = id;
            userInRole.RoleId = userVM.RoleId;
            var roleData = Mapper.UserInRoleMapper.Attach(userInRole);
            userData.UserInRoles.Add(roleData);
            _userService.SaveChanges();
            return RedirectToAction("userDetails");
        }

        [HttpGet]
        public ActionResult Index()
        {
            var users = _userService.Get(u => u.IsActive == true);
            var mappedUser = users.Select(s => new UserViewModel
            {
                UserId = s.UserId,
                UserName = s.UserName,
                Password = s.Password,
                Email = s.Email,
                Address=s.Address,
                MobileNo = Convert.ToString(s.MobileNo),
                CityName = s.CityMaster.CityName,
                RoleName=s.UserInRoles.Select(u=>u.RoleMaster.RoleName).FirstOrDefault(),
                CompanyMaster= s.CompanyMaster == null ? null : new CompanyViewModel()
                {
                    CompanyID = s.CompanyMaster.CompanyID,
                    CompanyName = s.CompanyMaster.CompanyName,
                    MobileNo = s.CompanyMaster.MobileNo,

                },
                CityMaster = s.CityMaster == null ? null : new CityViewModel()
                {
                    CityId = s.CityMaster.CityId,
                    CityName = s.CityMaster.CityName,
                    CityShortName = s.CityMaster.CityShortName,
                }
            }).ToList();           

            return View(mappedUser);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = _userService.GetUserById(id);
            var userInRole = _userInRoleService.Get().Where(w => w.UserMaster.UserInRoles.Where(we => we.RoleId == user.UserInRoles.Select(a => a.RoleId).FirstOrDefault()).Select(s => s.RoleId).Any()).FirstOrDefault().RoleId;
            Session["roleId"] = userInRole;
            var mappedUser = Mapper.UserMapper.Detach(user);
            var roles = _roleMasterService.Get(r => r.IsActive == true);
            var cities = _cityMasterService.Get(r => r.IsActive == true);
            var companies = _companyMasterService.Get(r => r.IsActive == true);

            mappedUser.RoleId = Convert.ToInt32(userInRole);
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
            ViewBag.Cities = new SelectList(cities, "CityId", "CityName");
            ViewBag.Companies = new SelectList(companies, "CompanyId", "CompanyName");

            return View(mappedUser);
        }
        [HttpPost]
        public ActionResult Edit(UserViewModel userVM)
        {
            if(ModelState.IsValid)
            {
                if(Session["roleId"] == null)
                {
                    return RedirectToAction("Index");
                }
                var userInRoleVM = new UserInRoleViewModel();
                var userInRoleMaster = new UserInRole();
                var userData = _userService.GetUserById(userVM.UserId);
                var userInRole = _userInRoleService.Get().Where(r => r.RoleId == Convert.ToInt32(Session["roleId"])).FirstOrDefault();

                userData.UserName = userVM.UserName;
                userData.Password = userVM.Password;
                userData.Email = userVM.Email;
                userData.MobileNo = userVM.MobileNo;
                userData.Address = userVM.Address;
                userData.CityId = userVM.CityId;
                userData.CompanyID = userVM.CompanyID;

                userInRole.RoleId = userVM.RoleId;
                userInRole.UserId = userVM.UserId;

                _userInRoleService.Update(userInRole);
                _userInRoleService.SaveChanges();
                _userService.Update(userData);
                _userService.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var users = _userService.Get(u => u.UserId == id).FirstOrDefault();
            var userModel = Mapper.UserMapper.Detach(users);
            userModel.RoleName = users.UserInRoles.Select(u => u.RoleMaster.RoleName).FirstOrDefault();
            return View(userModel);
        }
    }
}