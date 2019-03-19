using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPassBook.DAL.DBModel;

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
            var cities = _cityMasterService.Get().Select(s=>new CityViewModel { CityId=s.CityId,CityName=s.CityName,CityShortName=s.CityShortName,IsActive=s.IsActive}).ToList();    
            //var cityViewModel=Mapper.CityMapper.Detach(cities)                                                                     
            return View(cities);
        }
            //Get :Edit 
        [HttpGet]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Edit(int id)
        {
            var city = _cityMasterService.GetCityById(id);
            var cityViewModel = Mapper.CityMapper.Detach(city);
            return View(cityViewModel);
        }
        [HttpPost]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Edit(CityViewModel cityModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var citDbydata = _cityMasterService.GetCityById(cityModel.CityId);
            citDbydata.CityName = cityModel.CityName;
            citDbydata.CityShortName = cityModel.CityShortName;

            _cityMasterService.Update(citDbydata);
            _cityMasterService.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get: Create
        [HttpGet]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Create(CityViewModel cityModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            CityMaster cityMasterModel = new CityMaster();
            cityMasterModel.CityName = cityModel.CityName;
            cityMasterModel.CityShortName = cityModel.CityShortName;
            cityMasterModel.IsActive = true;

            _cityMasterService.Add(cityMasterModel);
            _cityMasterService.SaveChanges();
            return RedirectToAction("Index");
        }
         //Get: Delete
        
        [HttpGet]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Delete(int id)
        {
            var citDbydata = _cityMasterService.GetCityById(id);
            citDbydata.IsActive=false;
          
            _cityMasterService.Update(citDbydata);
            _cityMasterService.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        [CustomAuthorize(Common.Admin)]
        public ActionResult Details(int id)
        {
            var cityDbydata = _cityMasterService.Get(c => c.CityId == id).FirstOrDefault();
            var cityViewModel = Mapper.CityMapper.Detach(cityDbydata);
            return View(cityViewModel);
        }
    }

    
}
