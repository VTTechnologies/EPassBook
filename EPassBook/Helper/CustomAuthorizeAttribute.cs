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
        IUserService _userService;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            _userService = DependencyResolver.Current.GetService<IUserService>();
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            UserViewModel userDetail = new UserViewModel();
            userDetail = httpContext.Session["UserDetails"] as UserViewModel;
            foreach (var role in allowedroles)
            {
                if (userDetail != null)
                {
                    if (_userService.AuthenticateUser(userDetail.UserName, userDetail.Password, role))
                    {
                        authorize = true;
                    }
                }

            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Error/UnauthorizedAccess");
        }
    }
}