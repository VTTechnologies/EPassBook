﻿using EPassBook.DAL.IService;
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
        [CustomAuthorize(Common.DataEntry, Common.Admin)]
        public ActionResult Index()
        {
            int? companyId = 0;
            if (Session["UserDetails"] != null)
            {
                var admindata = Session["UserDetails"] as UserViewModel;
                companyId = admindata.CompanyID;
            }
            else
            {
                RedirectToAction("Login");
            }
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
        [CustomAuthorize(Common.DataEntry, Common.Admin)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beneficiary/Create
        [HttpPost]
        [CustomAuthorize(Common.DataEntry,Common.Admin)]
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

        public string SendSms(BeneficiaryViewModel beneficiaryViewModel)
        {
            string msg = "";
            try
            {
                var Link = "http://www.navnirmangroup.org/files/public-docs/app-debug.apk";


                string BeniUserName = beneficiaryViewModel.AdharNo.ToString();
                BeniUserName = !String.IsNullOrWhiteSpace(BeniUserName) && BeniUserName.Length >= 6 ? BeniUserName.Substring(BeniUserName.Length - 6) : BeniUserName;
                var BeniPassword = beneficiaryViewModel.Password;

                string key = "3r9mqo0k6few3m8";
                string secret = "mcadq4cuu96g6wp";
                string to = beneficiaryViewModel.MobileNo;
                string messages = "Click below link to download the App & use credentials for login Username = " + BeniUserName + " & Password = " + BeniPassword + " " + Link;
                string URL = "https://www.thetexting.com/rest/sms/json/message/send?api_key=" + key + "&api_secret=" + secret + "&to=" + to + "&text=" + messages;
            }
            catch (Exception ex)
            {
                msg = "Error";
            }
            return msg;
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
        [CustomAuthorize(Common.DataEntry, Common.Admin)]
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
