using AutoMapper;
using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EPassBook.Helper
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        EPassBookEntities context = new EPassBookEntities();

        IUserService _userService;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            UserViewModel uvm = new UserViewModel();            
            if(httpContext.Session["UserDetails"] !=null )
            {
                uvm = httpContext.Session["UserDetails"] as UserViewModel;

                foreach (var role in allowedroles)
                {
                    var user = context.UserMasters.Where(m => m.UserName == uvm.UserName & m.Password == uvm.Password
                    && m.UserInRoles.Where(r => r.RoleMaster.RoleName == role).Any() && m.IsActive == true);

                    if (user.Count() > 0)
                    {
                        authorize = true;
                    }
                }
                return authorize;
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Error/UnauthorizedAccess");
        }
    }
}