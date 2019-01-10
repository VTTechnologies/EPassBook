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
                            else
                            {
                                return RedirectToAction("Index", "WorkFlow");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The password provided is incorrect.");
                        Session["UserDetails"] = null;
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The username or password provided is incorrect.");
                    Session["UserDetails"] = null;
                    return RedirectToAction("Login");
                }
            }
            else
            {
                Session["UserDetails"] = null;
                return RedirectToAction("Login");
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

            GMailer.GmailUsername = "athersayed.vtt@gmail.com";
            GMailer.GmailPassword = "rahtasayed..";

            GMailer mailer = new GMailer();
            mailer.ToEmail = user.Email;
            mailer.Body = PopulateBody(user);
            mailer.IsHtml = true;
            mailer.Send();

            return View();
        }

        [HttpGet]
        public ActionResult sendMail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult sendMail(Mail mail)
        {
            //GMailer.GmailUsername = "athersayed.vtt@gmail.com";
            //GMailer.GmailPassword = "rahtasayed..";

            //GMailer mailer = new GMailer();
            //mailer.ToEmail = mail.ToEmail;
            //mailer.Subject = mail.Subject;
            //mailer.Body = PopulateBody(mail);
            //mailer.IsHtml = true;
            //mailer.Send();
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
            if(!ModelState.IsValid)
            {
                return View();
            }
            var fName = userVM.FirstName;
            var lName = userVM.LastName;
            var mobileNo = userVM.MobileNo;

            string firstCharOfFname = 
                !String.IsNullOrWhiteSpace(fName) && fName.Length >= 1? fName.Substring(0, 1) : fName;
            var userName = firstCharOfFname + userVM.LastName;

            string firstCharOfLname =
                !String.IsNullOrWhiteSpace(lName) && fName.Length >= 1 ? lName.Substring(0, 1) : lName;

            var password = firstCharOfFname + firstCharOfLname + mobileNo;
            password = password.Encrypt();
            userVM.IsActive = true;
            userVM.UserName = userName;
            userVM.Password = password;
            userVM.UserInRoles = new List<UserInRoleViewModel>();
            userVM.UserInRoles.Add(new UserInRoleViewModel() { RoleId= userVM.RoleId });
            var userData = Mapper.UserMapper.Attach(userVM);
            _userService.Add(userData);
            _userService.SaveChanges();
            //var roleData = Mapper.UserInRoleMapper.Attach(userInRole);
            //userData.UserInRoles.Add(roleData);
            //_userService.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Index()
        {
            var users = _userService.Get(u => u.IsActive == true);
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
                return View(userVM);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var users = _userService.Get(u => u.UserId == id).FirstOrDefault();
            var userModel = Mapper.UserMapper.Detach(users);
            userModel.RoleName = users.UserInRoles.Select(u => u.RoleMaster.RoleName).FirstOrDefault();
            userModel.Password = userModel.Password.Decrypt();
            return View(userModel);
        }
        [HttpGet]
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
        
    }
}