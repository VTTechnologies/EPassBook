using EPassBook.DAL.DBModel;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Mapper
{
    public static class UserInRoleMapper
    {

        public static UserInRole Attach(UserInRoleViewModel userInRoleViewModel)
        {

            UserInRole userInRole = new UserInRole();              
        
            return userInRole;
        }
        public static UserInRoleViewModel Detach(UserInRole userInRole)
        {
            UserInRoleViewModel userInRoleViewModel = new UserInRoleViewModel();            

            return userInRoleViewModel;
        }
    }
}