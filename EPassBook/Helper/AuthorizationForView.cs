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
    public class AuthorizationForView 
    {
       private static IUserService _userService;
       private static UserViewModel userDetail;


        public  AuthorizationForView(IUserService userService)
        {
            
            userDetail= new UserViewModel();
        }
        public  static bool IsAuthorize(params string[] roles)
        {
            _userService = DependencyResolver.Current.GetService<IUserService>();

            bool authorize = false;            
             userDetail = HttpContext.Current.Session["UserDetails"] as UserViewModel;
            foreach (var role in roles)
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
    }
}