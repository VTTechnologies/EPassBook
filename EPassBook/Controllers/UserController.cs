using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

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
        
        [HttpGet]
        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToAction("Login");
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
                UserViewModel uservm = new UserViewModel();

                if (user.RememberMe)
                {
                    HttpCookie userCookie = new HttpCookie("userinfo");
                    userCookie["user"] = user.UserName;
                    userCookie["password"] = user.Password;
                    userCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(userCookie);
                }
                if (userData != null)
                {
                    uservm = Mapper.UserMapper.Detach(userData);
                    Session["UserDetails"] = uservm;
                    var password = userData.Password.Decrypt();

                    if (user.Password.Equals(password))
                    {
                        Session["CompID"] = userData.CompanyID;
                        if (userData.IsReset != true)
                        {
                            return RedirectToAction("ResetPassword", "User");
                        }
                        else
                        {
                            if (uservm.UserInRoles.FirstOrDefault().RoleId == Convert.ToInt32(Common.Roles.DataEntry))
                            {
                                return RedirectToAction("Index", "Beneficiary");
                            }
                            else if (uservm.UserInRoles.FirstOrDefault().RoleId == Convert.ToInt32(Common.Roles.Admin))
                            {
                                return RedirectToAction("FakeDashboard", "Dashboard");
                            }
                            else
                            {
                                return RedirectToAction("Index", "WorkFlow");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "The password provided is incorrect.");
                        Session["UserDetails"] = null;
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "The username or password provided is incorrect.");
                    Session["UserDetails"] = null;
                    return View();
                }
            }
            else
            {
                Session["UserDetails"] = null;
                return View();
            }
        }
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
                        var password = userData.Password;

                        password = userData.Password.Decrypt();

                        if (password != resetPassVM.oldPassword)
                        {
                            ModelState.AddModelError(string.Empty, "Wrong old password");
                            return View(resetPassVM);
                        }
                        user.Password = resetPassVM.newPassword.Encrypt();
                        user.IsReset = true;
                        _userService.Update(user);
                        _userService.SaveChanges();

                        userData = Mapper.UserMapper.Detach(_userService.GetUserById(userData.UserId));
                        Session["UserDetails"] = userData;
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

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(FormCollection formCollection)
        {
            var Email = Request["Email"].ToString();
            if(Email == null)
            {
                Email = formCollection["Email"].ToString();
            }

            var user = new UserMaster();
            user = _userService.Get().Where(u => u.Email == Email).FirstOrDefault();

            if(user == null)
            {
                ViewData["Error"] = "Email does not exist";
                return RedirectToAction("ForgetPassword");
            }
            GMailer.GmailUsername = "";
            GMailer.GmailPassword = "";

            GMailer mailer = new GMailer();
            mailer.ToEmail = user.Email;
            mailer.Body = PopulateBody(user);
            mailer.IsHtml = true;
            mailer.Send();

            return View();
        }     
        public string PopulateBody(UserMaster user)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Shared/MailBody.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", user.UserName);
            body = body.Replace("{Title}", "Click here to reset your password");
            body = body.Replace("{Url}", "http://localhost:23488//User/ResetPassword");
            //body = body.Replace("{Description}", user.Description);
            return body;
        }
        
        [HttpGet]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Create()
        {
            var roles = _roleMasterService.Get(r => r.IsActive == true);
            var cities = _cityMasterService.Get(r => r.IsActive == true);
            UserViewModel um = new UserViewModel();
            um.IsActive = true;
            um.Roles = roles.Select(s => new SelectListItem { Text = s.RoleName, Value = s.RoleId.ToString() }).ToList();
            um.Cities = cities.Select(c => new SelectListItem { Text = c.CityName, Value = c.CityId.ToString() }).ToList();
            return View(um);
        }
        [HttpPost]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Create([Bind(Exclude = "UserName,Password,CompanyID")] UserViewModel userVM)
        {
            if (!ModelState.IsValidField("FirstName") && !ModelState.IsValidField("LastName")
                && !ModelState.IsValidField("Dob") && !ModelState.IsValidField("Password")
                && !ModelState.IsValidField("RoleId") && !ModelState.IsValidField("Email")
                && !ModelState.IsValidField("MobileNo") && !ModelState.IsValidField("CityId"))
            { 
                var roles = _roleMasterService.Get(r => r.IsActive == true);
                var cities = _cityMasterService.Get(r => r.IsActive == true);
                UserViewModel um = new UserViewModel();
                um.Roles = roles.Select(s => new SelectListItem { Text = s.RoleName, Value = s.RoleId.ToString() }).ToList();
                um.Cities = cities.Select(c => new SelectListItem { Text = c.CityName, Value = c.CityId.ToString() }).ToList();
                um.IsActive = true;
                return View(um);
            }
            int? companyId = 0;
            if(Session["UserDetails"] != null)
            {
                var admindata = Session["UserDetails"] as UserViewModel;
                companyId = admindata.CompanyID;
            }
            else
            {
                RedirectToAction("Login");
            }
            if(CheckUserByCityAndRole(userVM))
            {
                ModelState.AddModelError("UserExist", "User already exist for selected role. Please change the city or role");
                var roles = _roleMasterService.Get(r => r.IsActive == true);
                var cities = _cityMasterService.Get(r => r.IsActive == true);
                userVM.Roles = roles.Select(s => new SelectListItem { Text = s.RoleName, Value = s.RoleId.ToString() }).ToList();
                userVM.Cities = cities.Select(c => new SelectListItem { Text = c.CityName, Value = c.CityId.ToString() }).ToList();
                userVM.IsActive = true;
                return View(userVM);
            }
            var fName = userVM.FirstName;
            var lName = userVM.LastName;
            var mobileNo = userVM.MobileNo;

            string firstCharOfFname =
                !String.IsNullOrWhiteSpace(fName) && fName.Length >= 1 ? fName.Substring(0, 1) : fName;
            var userName = firstCharOfFname + userVM.LastName;

            string firstCharOfLname =
                !String.IsNullOrWhiteSpace(lName) && fName.Length >= 1 ? lName.Substring(0, 1) : lName;

            var password = firstCharOfFname + firstCharOfLname + mobileNo;
            password = password.Encrypt();
            userVM.IsActive = true;
            userVM.UserName = userName;
            userVM.Password = password;
            userVM.CompanyID = companyId;
            userVM.UserInRoles = new List<UserInRoleViewModel>();
            userVM.UserInRoles.Add(new UserInRoleViewModel() { RoleId= userVM.RoleId });
            var userData = Mapper.UserMapper.Attach(userVM);
            _userService.Add(userData);
            _userService.SaveChanges();
            return RedirectToAction("Index");
        }       

        [HttpGet]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Index()
        {
            int? companyId = 0;
            if (Session["UserDetails"] != null)
            {
                var admindata = Session["UserDetails"] as UserViewModel;
                companyId = admindata.CompanyID;
            }
            else
            {
                RedirectToAction("Login");
            }
            var users = _userService.Get(u => u.IsActive == true & u.CompanyID == companyId, u => u.OrderBy(o => o.UserId), "");
            var mappedUser = users.Select(s => new UserViewModel
            {
                UserId = s.UserId,
                UserName = s.UserName,
                Password = s.Password.Decrypt(),
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
        [CustomAuthorize(Common.Admin)]
        public ActionResult Edit(int id)
        {
            var user = _userService.GetUserById(id);
            var userInRole = _userInRoleService.Get().Where(w => w.UserMaster.UserInRoles.Where(we => we.RoleId == user.UserInRoles.Select(a => a.RoleId).FirstOrDefault()).Select(s => s.RoleId).Any()).FirstOrDefault().RoleId;
            Session["roleId"] = userInRole;
            var mappedUser = Mapper.UserMapper.Detach(user);
            mappedUser.Password = mappedUser.Password.Decrypt();
            var roles = _roleMasterService.Get(r => r.IsActive == true);
            var cities = _cityMasterService.Get(r => r.IsActive == true);
            var companies = _companyMasterService.Get(r => r.IsActive == true);
            mappedUser.RoleId = Convert.ToInt32(userInRole);

            mappedUser.Roles = roles.Select(s=>new SelectListItem {Text=s.RoleName,Value=s.RoleId.ToString() }).ToList();
            mappedUser.Cities = cities.Select(c => new SelectListItem { Text = c.CityName, Value = c.CityId.ToString() }).ToList();

            //ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
            //ViewBag.Cities = new SelectList(cities, "CityId", "CityName");
            //ViewBag.Companies = new SelectList(companies, "CompanyId", "CompanyName");
            
            return View(mappedUser);
        }
        [HttpPost]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Edit([Bind(Exclude = "UserName")] UserViewModel userVM)
        {
            if(ModelState.IsValidField("FirstName")&& ModelState.IsValidField("LastName")
                && ModelState.IsValidField("Dob")&& ModelState.IsValidField("Password")
                && ModelState.IsValidField("RoleId") && ModelState.IsValidField("Email")
                && ModelState.IsValidField("MobileNo")&& ModelState.IsValidField("CityId"))
            {
                if (CheckUserByCityAndRole(userVM))
                {
                    ModelState.AddModelError("UserExist", "User already exist for selected role. Please change the city or role");
                    var roles = _roleMasterService.Get(r => r.IsActive == true);
                    var cities = _cityMasterService.Get(r => r.IsActive == true);
                    userVM.Roles = roles.Select(s => new SelectListItem { Text = s.RoleName, Value = s.RoleId.ToString() }).ToList();
                    userVM.Cities = cities.Select(c => new SelectListItem { Text = c.CityName, Value = c.CityId.ToString() }).ToList();
                    userVM.IsActive = true;
                    return View(userVM);
                }

                var fName = userVM.FirstName;
                var lName = userVM.LastName;
                var mobileNo = userVM.MobileNo;

                string firstCharOfFname =
                !String.IsNullOrWhiteSpace(fName) && fName.Length >= 1 ? fName.Substring(0, 1) : fName;
                var userName = firstCharOfFname + userVM.LastName;

                if (Session["roleId"] == null)
                {
                    return RedirectToAction("Index");
                }
                var userInRoleVM = new UserInRoleViewModel();
                var userInRoleMaster = new UserInRole();
                var userData = _userService.GetUserById(userVM.UserId);
                var userInRole = _userInRoleService.Get().Where(r => r.RoleId == Convert.ToInt32(Session["roleId"])).FirstOrDefault();

                userData.UserName = userName;
                userData.Password = userVM.Password.Encrypt();
                userData.Email = userVM.Email;
                userData.MobileNo = userVM.MobileNo;
                userData.Address = userVM.Address;
                userData.CityId = userVM.CityId;
                userData.FirstName = userVM.FirstName;
                userData.LastName = userVM.LastName;
                userData.Dob = userVM.Dob;
                //userData.CompanyID = userVM.CompanyID;

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
                var roles = _roleMasterService.Get(r => r.IsActive == true);
                var cities = _cityMasterService.Get(r => r.IsActive == true);

                userVM.Roles = new List<SelectListItem>();
                userVM.Cities = new List<SelectListItem>();

                userVM.Roles = roles.Select(s => new SelectListItem { Text = s.RoleName, Value = s.RoleId.ToString() }).ToList();
                userVM.Cities = cities.Select(c => new SelectListItem { Text = c.CityName, Value = c.CityId.ToString() }).ToList();

                return View(userVM);
            }
        }

        [HttpGet]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Details(int id)
        {
            var users = _userService.Get(u => u.UserId == id).FirstOrDefault();
            var userModel = Mapper.UserMapper.Detach(users);
            userModel.RoleName = users.UserInRoles.Select(u => u.RoleMaster.RoleName).FirstOrDefault();
            userModel.Password = userModel.Password.Decrypt();
            return View(userModel);
        }
        [HttpGet]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Delete(int id)
        {
            if (id > 0 || !string.IsNullOrWhiteSpace(id.ToString()))
            {
                var user = _userService.Get(u => u.UserId == id).FirstOrDefault();
                user.IsActive = false;
                _userService.Update(user);
                _userService.SaveChanges();
                return RedirectToAction("Index");                
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [CustomAuthorize(Common.Admin)]
        public bool CheckUserByCityAndRole(UserViewModel userVM)
        {
            int roleId = userVM.RoleId;
            int userid = userVM.UserId;
            bool exist = false;
            var listOfUsers = _userService.GetAllUsers().Where(u => u.CityId == userVM.CityId && u.IsActive == true).ToList();

            if (roleId == (int)Common.Roles.DataEntry) // no validation for data entry user
            {
                return exist;
            }
            if (userid == 0) //validation for create user
            {
                if (listOfUsers.Count > 0)
                {
                    foreach (var user in listOfUsers)
                    {
                        var exst = user.UserInRoles.Where(u => u.RoleId == roleId).Any();
                        if (exst)
                        {
                            exist = true;
                            break;
                        }
                    }
                }
            }
            if (userid != 0) //validation for Edit user
            {
                if (listOfUsers.Count > 0)
                {
                    foreach (var user in listOfUsers)
                    {
                        var exst = user.UserInRoles.Where(u => u.RoleId == roleId).Select(r => r.UserId).FirstOrDefault();
                        if (exst != null)
                        {
                            if (exst == userid)
                            {
                                exist = false;
                            }
                            else
                            {
                                exist = true;
                            }
                            break;
                        }
                    }
                }
            }
            return exist;
        }
    }
}