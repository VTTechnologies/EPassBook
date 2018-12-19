using AutoMapper;
using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPassBook.Controllers
{
    public class WorkFlowController : Controller
    {
        private readonly IMapper _mapper;
        IInstallmentDetailService _installmentDetailService;

        public WorkFlowController(IMapper mapper, IInstallmentDetailService installmentDetailService)
        {
            _installmentDetailService = installmentDetailService;
            _mapper = mapper;
        }
        // GET: WorkFlow
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateAccountant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccountant(InstallmentDetailsViewModel installmentDetailViewModel)
        {
            var installmet = _installmentDetailService.GetInstallmentDetailById(12);

            installmet.TransactionID = installmentDetailViewModel.TransactionID;

            var comment = new Comment();
            comment.Comments = installmentDetailViewModel._Comments;
            comment.CompanyID = 1;

            installmet.Comments.Add(comment);

            installmet.ModifiedBy = "Admin";
            installmet.ModifiedDate = DateTime.Now;



            _installmentDetailService.Update(installmet);
            _installmentDetailService.SaveChanges();

            return View();
        }
    }
}