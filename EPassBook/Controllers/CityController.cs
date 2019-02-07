using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPassBook.Controllers
{
    [ElmahError]
    public class CityController : Controller
    {
        ICityService _cityMasterService;

        public CityController(ICityService cityMasterService)
        {
            _cityMasterService = cityMasterService;          
        }
        // GET: Beneficiary
        [CustomAuthorize(Common.Admin)]
        public ActionResult Index()
        {           
            var cities = _cityMasterService.Get(c => c.IsActive == true).Select(s=>new CityViewModel { CityId=s.CityId,CityName=s.CityName,CityShortName=s.CityShortName,IsActive=s.IsActive}).ToList();    
            //var cityViewModel=Mapper.CityMapper.Detach(cities)
            return View(cities);
        }

       
    }
}
