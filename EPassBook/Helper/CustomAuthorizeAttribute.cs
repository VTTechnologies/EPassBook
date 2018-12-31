using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security;
using System.Web.Mvc.Filters;

namespace EPassBook.Helper
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        IUserService _userService;
        UserViewModel userDetail;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            _userService = DependencyResolver.Current.GetService<IUserService>();
            this.allowedroles = roles;
            userDetail= new UserViewModel();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
           
            userDetail = httpContext.Session["UserDetails"] as UserViewModel;
            foreach (var role in allowedroles)
            {
                if (userDetail != null)
                {
                    if (_userService.Get(w => w.UserName == userDetail.UserName && w.Password == userDetail.Password && w.UserInRoles.Where(r => r.RoleMaster.RoleName == role).Any() && w.IsActive == true, null, string.Empty).Any())
                    {
                        authorize = true;
                    }
                }

            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //userDetail
            if (filterContext.HttpContext.Session["UserDetails"] == null)
            {
                filterContext.Result = new RedirectResult("~/User/Login");
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Error/UnauthorizedAccess");
            }
        }
    }
}