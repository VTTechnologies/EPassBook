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
    public class BeneficiaryController : Controller
    {
        IBenificiaryService _benificiaryService;
        IInstallmentDetailService _installmentDetailService;

        public BeneficiaryController(IInstallmentDetailService installmentDetailService, IBenificiaryService benificiaryService)
        {
            _benificiaryService = benificiaryService;
            _installmentDetailService = installmentDetailService;
        }
        // GET: Beneficiary
        public ActionResult Index()
        {
            var beneficiaries = _benificiaryService.GetAllBenificiaries();
            var beneficiaryviewmodel = beneficiaries.Select(s => new BeneficiaryViewModel
            {
                BeneficiaryId = s.BeneficiaryId,
                BeneficairyName = s.BeneficairyName,
                AdharNo = s.AdharNo,
                MobileNo = s.MobileNo
            }).ToList();
            return View(beneficiaryviewmodel);
        }

        // GET: Beneficiary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Beneficiary/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beneficiary/Create
        [HttpPost]
        [CustomAuthorize(Common.DataEntry)]
        public ActionResult Create(BeneficiaryViewModel beneficiaryViewModel)
        {
            try
            {
                HttpPostedFileBase hasbandphoto = Request.Files["imgupload1"];
                HttpPostedFileBase wifephoto = Request.Files["imgupload2"];
                var user = Session["UserDetails"] as UserViewModel;
                string hphoto = PhotoManager.savePhoto(hasbandphoto, 0, "Benificiary");
                string wphoto = PhotoManager.savePhoto(wifephoto, 0, "Benificiary");
                beneficiaryViewModel.Hasband_Photo = hphoto; //PhotoManager.ConvertToBytes(hasbandphoto);
                beneficiaryViewModel.Wife_Photo = wphoto; //PhotoManager.ConvertToBytes(wifephoto);
                beneficiaryViewModel.CreatedBy = user.UserName;
                beneficiaryViewModel.CreatedDate = DateTime.Now;
                var insertbeneficiary = Mapper.BeneficiaryMapper.Attach(beneficiaryViewModel);

                _benificiaryService.Add(insertbeneficiary);
                _benificiaryService.SaveChanges();

                ViewBag.Message = "sussess message";

                //return RedirectToAction("Index");

            }
            catch
            {

            }
            return View();
        }

        // GET: Beneficiary/Edit/5
        public ActionResult Edit(int id)
        {
            BeneficiaryViewModel beneficiaryViewModel = new BeneficiaryViewModel();
            var beneficiarybyid = _benificiaryService.GetBenificiaryById(id);
            var beneficiaryvm = Mapper.BeneficiaryMapper.Detach(beneficiarybyid);


            ViewBag.hsbphoto = "/Uploads/BeneficiaryImages/" + beneficiaryvm.Hasband_Photo;
            ViewBag.wfphoto = "/Uploads/BeneficiaryImages/" + beneficiaryvm.Wife_Photo;
            return View("Edit", beneficiaryvm);
        }

        // POST: Beneficiary/Edit/5
        [HttpPost]
        [CustomAuthorize(Common.DataEntry)]
        public ActionResult Edit(int id, BeneficiaryViewModel beneficiaryViewModel)
        {
            try
            {
                HttpPostedFileBase hasbandphoto = Request.Files["imgupload1"];
                HttpPostedFileBase wifephoto = Request.Files["imgupload2"];
                var user = Session["UserDetails"] as UserViewModel;
                string hphoto = PhotoManager.savePhoto(hasbandphoto, 0, "Benificiary");
                string wphoto = PhotoManager.savePhoto(wifephoto, 0, "Benificiary");
                beneficiaryViewModel.Hasband_Photo = hphoto; //PhotoManager.ConvertToBytes(hasbandphoto);
                beneficiaryViewModel.Wife_Photo = wphoto; //PhotoManager.ConvertToBytes(wifephoto);
                beneficiaryViewModel.CreatedBy = user.UserName;
                var insertbeneficiary = Mapper.BeneficiaryMapper.Attach(beneficiaryViewModel);


                _benificiaryService.Update(insertbeneficiary);

                ViewBag.Message = "sussess message";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Beneficiary/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Beneficiary/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id > 0 || !string.IsNullOrWhiteSpace(id.ToString()))
            {
                _benificiaryService.Delete(id);
                _benificiaryService.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
