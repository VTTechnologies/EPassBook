using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPassBook.Models;
using EPassBook.Helper;

namespace EPassBook.Controllers
{
    [ElmahError]
    public class DashboardController : Controller
    {
        IBenificiaryService _iBenificiaryService;
        ICityService _icityService;
        IInstallmentDetailService _iInstallmentDetailService;
       
        // GET: Dashboard
        public DashboardController(IInstallmentDetailService installmentDetailService,  ICityService cityMasterService,
        IBenificiaryService beneficiaryService)
        {
            _iBenificiaryService = beneficiaryService;
            _icityService = cityMasterService;
            _iInstallmentDetailService = installmentDetailService;
        }
        [CustomAuthorize(Common.Admin)]
        public ActionResult Dashboard()
        {
            ViewBag.Cities = _icityService.GetAllCities().Select(s => new SelectListItem { Text=s.CityName, Value = s.CityId.ToString() }).ToList();
            return View(new ReportViewModel());
        }      

        [HttpGet]
        [CustomAuthorize(Common.Admin)]
        public ActionResult MasterReports()
        {
            //var cities = _cityMasterService.Get(r => r.IsActive == true);
            //var citiesSelectList = cities.Select(c => new SelectListItem { Text = c.CityName, Value = c.CityId.ToString() }).ToList();
            //ViewBag.Cities = citiesSelectList;
            return View(new List<WorkStatusDetailsViewModel>());
        }
        [HttpPost]
        [CustomAuthorize(Common.Admin)]
        public JsonResult MasterReports(int cityId, string DTRno)
        {
            var user = Session["UserDetails"] as UserViewModel;

            List<WorkStatusDetailsViewModel> workstatus = new List<WorkStatusDetailsViewModel>();
            var installments = _iInstallmentDetailService.Get(w => w.BenificiaryMaster.CityId == cityId && w.BenificiaryMaster.DTRNo == DTRno, w => w.OrderByDescending(o => o.InstallmentNo), "").ToList();
            var installmentslists = installments.GroupBy(o => o.InstallmentNo).
                Select(g => new
                {
                    InstallmentKey = g.Key,

                    Installment = g.Select(i => i.InstallmentNo).FirstOrDefault() == 1 ? "First" : g.Select(i => i.InstallmentNo).FirstOrDefault() == 2 ? "Second" : g.Select(i => i.InstallmentNo).FirstOrDefault() == 3 ? "Third" :
                                  g.Select(i => i.InstallmentNo).FirstOrDefault() == 4 ? "Fourth" : g.Select(i => i.InstallmentNo).FirstOrDefault() == 5 ? "Fifth" : g.Select(i => i.InstallmentNo).FirstOrDefault() == 6 ? "Sixth-Cum Final" : "",
                    LevelType = g.Select(i => i.InstallmentNo).FirstOrDefault() == 1 ? "At Plinth Level" : g.Select(i => i.InstallmentNo).FirstOrDefault() == 2 ? "At Lintel Level" : g.Select(i => i.InstallmentNo).FirstOrDefault() == 3 ? "At Roof Level" :
                                g.Select(i => i.InstallmentNo).FirstOrDefault() == 4 ? "For Finishing Completion" : g.Select(i => i.InstallmentNo).FirstOrDefault() == 5 ? "Level" : g.Select(i => i.InstallmentNo).FirstOrDefault() == 6 ? "Level" : "",
                    BeneficiaryAmount = g.Sum(i => i.BeneficiaryAmnt),
                    CenterAmount = g.Where(i => i.IsCentreAmnt == true).Sum(l => l.LoanAmnt),
                    StateAmount = g.Where(i => i.IsCentreAmnt == false).Sum(l => l.LoanAmnt),
                    ULBAmount = 0,
                    TotalAmount = g.Sum(b => b.BeneficiaryAmnt) + g.Sum(b => b.LoanAmnt),
                }).ToList();

            ViewBag.GrandTotal = installmentslists.Sum(t => t.TotalAmount);

            //if (installments != null)
            //{
            //    workstatus = installments.Select(s => new WorkStatusDetailsViewModel
            //    {
            //        Installment = s.InstallmentNo == 1 ? "First" : s.InstallmentNo == 2 ? "Second" : s.InstallmentNo == 3 ? "Third" : s.InstallmentNo == 4 ? "Fourth" : s.InstallmentNo == 5 ? "Fifth" : s.InstallmentNo == 6 ? "Sixth-Cum Final" : "",
            //        LevelType = s.InstallmentNo == 1 ? "At Plinth Level" : s.InstallmentNo == 2 ? "At Lintel Level" : s.InstallmentNo == 3 ? "At Roof Level" : s.InstallmentNo == 4 ? "For Finishing Completion" : s.InstallmentNo == 5 ? "Level" : s.InstallmentNo == 6 ? "Level" : "",
            //        BeneficiaryAmount = s.BeneficiaryAmnt == null ? 0 : s.BeneficiaryAmnt,
            //        CenterAmount = s.IsCentreAmnt == null ? 0 : s.IsCentreAmnt == true ? s.LoanAmnt : 0,
            //        StateAmount = s.IsCentreAmnt == null ? 0 : s.IsCentreAmnt == false ? s.LoanAmnt : 0,
            //        ULBAmount = 0,
            //        TotalAmount = s.BeneficiaryAmnt + s.LoanAmnt == null ? 0 : s.BeneficiaryAmnt + s.LoanAmnt,
            //    }).ToList();

            //    ViewBag.GrandTotal = workstatus.Sum(w => w.TotalAmount);
            //}

            return Json(installmentslists);
        }
        [HttpPost]
        public JsonResult getAllCities()
        {

            var user = Session["UserDetails"] as UserViewModel;
            var cities = _icityService.Get(r => r.IsActive == true);
            var citiesSelectList = cities.Select(c => new SelectListItem { Text = c.CityName, Value = c.CityId.ToString()}).ToList();

            return Json(citiesSelectList);
        }
        [HttpPost]
        public JsonResult GetAllDTRNosByCity(int cityId)
        {
            var user = Session["UserDetails"] as UserViewModel;

            var dtrNos = _iBenificiaryService.Get().Where(c => c.CityId == cityId).GroupBy(d => d.DTRNo).Select(d => d.First());
            var drtsSelectList = dtrNos.Select(s => new SelectListItem { Text = s.DTRNo, Value = s.DTRNo }).ToList();
            return Json(drtsSelectList);
        }

        [HttpGet]
        public JsonResult FetchData(int cityId, string reportType)
        {
            var result = new List<SelectListItem>();
            switch (reportType)
            {
                case "DTR-Wise":
                    result = _iBenificiaryService.Get(w => w.CityId == cityId, null).GroupBy(x => x.DTRNo).Select((s, indexer) => new SelectListItem { Text = s.Key, Value = s.Count().ToString() }).ToList();
                    break;
                case "Caste-Wise":
                    result = _iBenificiaryService.Get(w => w.CityId == cityId, null).GroupBy(x => x.General).Select(s => new SelectListItem { Text = s.Key, Value = s.Count().ToString() }).ToList();
                    break;
                case "Installment-Wise":
                    result = _iInstallmentDetailService.Get(w => w.BenificiaryMaster.CityId == cityId, null, "BenificiaryMaster").GroupBy(x => x.InstallmentNo).Select(s => new SelectListItem { Text = s.Key.ToString(), Value = s.Sum(su => su.LoanAmnt).ToString() }).ToList();
                    break;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}