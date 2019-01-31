using EPassBook.DAL.DBModel;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Mapper
{
    public static class CityMapper
    {

        public static CityMaster Attach(CityViewModel cityViewModel)
        {

            CityMaster cityMaster = new CityMaster();
            cityMaster.CityId = cityViewModel.CityId;
            cityMaster.CityName   = cityViewModel.CityName;
            cityMaster.CityShortName = cityViewModel.CityShortName;
            cityMaster.IsActive = cityViewModel.IsActive;
            
        
            return cityMaster;
        }
        public static CityViewModel Detach(CityMaster cityMaster)
        {
            CityViewModel cityViewModel = new CityViewModel();
            cityViewModel.CityId = cityMaster.CityId;
            cityViewModel.CityName = cityMaster.CityName;
            cityViewModel.CityShortName = cityMaster.CityShortName;
            cityViewModel.IsActive = cityMaster.IsActive;

            return cityViewModel;
        }
    }
}