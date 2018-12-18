using AutoMapper;
using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using System;
using System.Web.Mvc;

namespace EPassBook.Controllers
{
    [ElmahError]
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        IUserService _userser;
        IBenificiary _Ibenificiary;
        
        public HomeController(IUserService userser, IBenificiary Ibenificiary, IMapper mapper)
        {
            _userser = userser;
            _Ibenificiary = Ibenificiary;
            _mapper = mapper;
        }

        [CustomAuthorize(Common.Admin)]
        public ActionResult Index()
        {
          
            return View();
        }
        //added by ather
        public ActionResult Website()
        {

            return View();
        }
        //end ather code

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult _InstallmentDetails()
        {
            var benfici = _Ibenificiary.GetBenificiaryById(1);            
            var benficiarymodel = _mapper.Map<BenificiaryMaster, BeneficiaryViewModel>(benfici);
            DateTime currentdate = Convert.ToDateTime(benficiarymodel.CreatedDate);            

            return PartialView(benficiarymodel);
        }
    }
}