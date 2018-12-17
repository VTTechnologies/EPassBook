using AutoMapper;
using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            foreach (var role in allowedroles)
            {
                var user = context.UserMasters.Where(m => m.UserName == uvm.UserName & m.Password == uvm.Password
                && m.RoleMaster.RoleName == role && m.IsActive == true);


                if (user.Count() > 0)
                {
                    authorize = true; /* return true if Entity has current user(active) with specific role */
                }
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}