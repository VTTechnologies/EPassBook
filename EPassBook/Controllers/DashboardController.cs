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
    public class DashboardController : Controller
    {
        IBenificiaryService _iBenificiaryService;
        ICityService _cityService;
        IInstallmentDetailService _iInstallmentDetailService;

        public DashboardController(IBenificiaryService iBenificiaryService, ICityService cityService, IInstallmentDetailService iInstallmentDetailService)
        {
            _iBenificiaryService = iBenificiaryService;
            _cityService = cityService;
            _iInstallmentDetailService = iInstallmentDetailService;
        }
        // GET: Dashboard
        public ActionResult Dashboard()
        {
            ViewBag.Cities = _cityService.GetAllCities().Select(s => new SelectListItem { Text=s.CityName, Value = s.CityId.ToString() }).ToList();
            return View(new ReportViewModel());
        }
        public ActionResult FakeDashboard()
        {
            return View();
        }

        public JsonResult FetchData(int cityId,string reportType) 
        {
            var result = new List<SelectListItem>();
            switch (reportType)
            {
                case "DTR-Wise":
                    result = _iBenificiaryService.Get(w => w.CityId == cityId, null).GroupBy(x => x.DTRNo).Select((s,indexer) => new SelectListItem { Text = s.Key, Value = s.Count().ToString() }).ToList();
                    break;
                case "Caste-Wise":
                    result = _iBenificiaryService.Get(w => w.CityId == cityId, null).GroupBy(x => x.General).Select(s => new SelectListItem { Text = s.Key, Value = s.Count().ToString() }).ToList();
                    break;
                case "Installment-Wise":
                    result = _iInstallmentDetailService.Get(w => w.BenificiaryMaster.CityId == cityId, null, "BenificiaryMaster").GroupBy(x => x.InstallmentNo).Select(s => new SelectListItem { Text = s.Key.ToString(), Value = s.Sum(su=>su.LoanAmnt).ToString() }).ToList();
                    break;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}