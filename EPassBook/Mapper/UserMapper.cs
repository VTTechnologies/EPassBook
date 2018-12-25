﻿using EPassBook.DAL.DBModel;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Mapper
{
    public static class UserMapper
    {

        public static UserMaster Attach(UserViewModel userViewModel)
        {

            UserMaster userMaster = new UserMaster();
            userMaster.UserId = userViewModel.UserId;
            userMaster.UserName = userViewModel.UserName;
            userMaster.Password = userViewModel.Password;
            userMaster.Email = userViewModel.Email;
            userMaster.MobileNo = userViewModel.MobileNo;
            userMaster.Address = userViewModel.Address;
            userMaster.IsActive = userViewModel.IsActive;
            userMaster.IsLoggedIn = userViewModel.IsLoggedIn;
            userMaster.CityId = userViewModel.CityId;
            userMaster.CompanyID = userViewModel.CompanyID;
            userMaster.IsReset = userViewModel.IsReset;
            userMaster.CityMaster = userViewModel.CityMaster == null ? null : new CityMaster()
            {
                CityId = userViewModel.CityMaster.CityId,
                CityName = userViewModel.CityMaster.CityName,
                CityShortName = userViewModel.CityMaster.CityShortName,
                IsActive = userViewModel.CityMaster.IsActive,

            };
            userMaster.CompanyMaster = userViewModel.CityMaster == null ? null : new CompanyMaster()
            {
                CompanyID = userViewModel.CompanyMaster.CompanyID,
                CompanyName = userViewModel.CompanyMaster.CompanyName,
                MobileNo = userViewModel.CompanyMaster.MobileNo,
                IsActive = userViewModel.CompanyMaster.IsActive,

            };
            userMaster.IsReset = userViewModel.IsReset;       
        
            return userMaster;
        }
        public static UserViewModel Detach(UserMaster userMaster)
        {
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.UserId = userMaster.UserId;
            userViewModel.UserName = userMaster.UserName;
            userViewModel.Password = userMaster.Password;
            userViewModel.Email = userMaster.Email;
            userViewModel.MobileNo = userMaster.MobileNo;
            userViewModel.Address = userMaster.Address;
            userViewModel.IsActive = userMaster.IsActive;
            userViewModel.IsLoggedIn = userMaster.IsLoggedIn;
            userViewModel.CityId = userMaster.CityId;
            userViewModel.CompanyID = userMaster.CompanyID;
            userViewModel.IsReset = userMaster.IsReset;
            userViewModel.CityMaster = userMaster.CityMaster == null ? null : new CityViewModel()
            {
                CityId = userMaster.CityMaster.CityId,
                CityName = userMaster.CityMaster.CityName,
                CityShortName = userMaster.CityMaster.CityShortName,
                IsActive = userMaster.CityMaster.IsActive,

            };
            userViewModel.CompanyMaster = userMaster.CityMaster == null ? null : new CompanyViewModel()
            {
                CompanyID = userMaster.CompanyMaster.CompanyID,
                CompanyName = userMaster.CompanyMaster.CompanyName,
                MobileNo = userMaster.CompanyMaster.MobileNo,

            };
            userViewModel.IsReset = userMaster.IsReset;

            return userViewModel;
        }
    }
}