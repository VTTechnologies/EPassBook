using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using EPassBook.DAL.DBModel;
using System.Data.SqlClient;
using System.Web.UI;
using System.Text;
using System.Net;

namespace EPassBook.Controllers
{
    [ElmahError]
    public class BeneficiaryController : Controller
    {
        IBenificiaryService _benificiaryService;
        ICityService _cityMasterService;
        IInstallmentDetailService _installmentDetailService;

        public BeneficiaryController(IInstallmentDetailService installmentDetailService, IBenificiaryService benificiaryService,
            ICityService cityMasterService)
        {
            _cityMasterService = cityMasterService;
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

            var cities = _cityMasterService.Get(c => c.IsActive == true);
            TempData["Cities"] = cities.Select(s => new SelectListItem { Text = s.CityName, Value = s.CityName }).ToList();

            var beneficiaries = _benificiaryService.GetAllBenificiaries();
            var beneficiaryviewmodel = beneficiaries.Select(s => new BeneficiaryViewModel
            {
                BeneficiaryId = s.BeneficiaryId,
                BeneficairyName = s.BeneficairyName,
                AdharNo = s.AdharNo,
                MobileNo = s.MobileNo,
                CityName = s.CityMaster == null ? "" : s.CityMaster.CityName
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
        [CustomAuthorize(Common.DataEntry, Common.Admin)]
        public ActionResult Create(BeneficiaryViewModel beneficiaryViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpPostedFileBase hasbandphoto = Request.Files["imgupload1"];
                HttpPostedFileBase wifephoto = Request.Files["imgupload2"];
                var user = Session["UserDetails"] as UserViewModel;
                string hphoto = PhotoManager.savePhoto(hasbandphoto, 0, "Benificiary");
                string wphoto = PhotoManager.savePhoto(wifephoto, 0, "Benificiary");
                beneficiaryViewModel.Hasband_Photo = hphoto; //PhotoManager.ConvertToBytes(hasbandphoto);
                beneficiaryViewModel.Wife_Photo = wphoto; //PhotoManager.ConvertToBytes(wifephoto);
                beneficiaryViewModel.CreatedBy = user.UserName;
                beneficiaryViewModel.CreatedDate = DateTime.Now;
                beneficiaryViewModel.CityId = user.CityId;
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

        [HttpGet]
        public ActionResult SendSMS(int id)
        {
            VerifyUser verifyUser = new VerifyUser();
            string response = string.Empty;
            string sid = "";// System.Configuration.ConfigurationManager.AppSettings["sid"];
            string user = System.Configuration.ConfigurationManager.AppSettings["user"];
            string password = System.Configuration.ConfigurationManager.AppSettings["password"];

            var beneficiaryViewModel = _benificiaryService.GetBenificiaryById(id);
            if (beneficiaryViewModel == null)
            {
                TempData["Message"] = "Some error occured please try again";
                TempData.Keep();
                return RedirectToAction("Index", "Beneficiary");
            }
            var Link = "http://www.navnirmangroup.org/files/public-docs/app-debug.apk";

            string BeniUserName = beneficiaryViewModel.AdharNo.ToString();
            BeniUserName = !String.IsNullOrWhiteSpace(BeniUserName) && BeniUserName.Length >= 6 ? BeniUserName.Substring(BeniUserName.Length - 6) : BeniUserName;
            var BeniPassword = beneficiaryViewModel.Password;
            string message = "Click below link to download the App and use credentials for login Username = " + BeniUserName + " and Password = " + BeniPassword + "  " + Link;
            string msisdn = beneficiaryViewModel.MobileNo;
            
            //string urlPromotional = "http://sms.sminfomedia.in/vendorsms/pushsms.aspx?user="+ user + "&password=" + password + "&msisdn=" + msisdn + "&sid=" + sid + "&msg=" + OTP + "&fl=0 ";
            string urlTransactional = "http://sms.sminfomedia.in/vendorsms/pushsms.aspx?user=" + user + "&password=" + password + "&msisdn=" + msisdn + "&sid=" + sid + "&msg=" + message + "&fl=0&gwid=2 ";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(urlTransactional);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                TempData["Message"] = "Message Sent";
                TempData.Keep();
                return RedirectToAction("Index", "Beneficiary");
            }
            else
            {
                TempData["Message"] = "Some error occured please try again";
                TempData.Keep();
                return RedirectToAction("Index", "Beneficiary");
            }
        }

        //useless
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
        public ActionResult Edit(BeneficiaryViewModel beneficiaryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            HttpPostedFileBase hasbandphoto = Request.Files["imgupload1"];
            HttpPostedFileBase wifephoto = Request.Files["imgupload2"];
            //var beniMaster = _benificiaryService.GetBenificiaryById(beneficiaryViewModel.BeneficiaryId);
            var user = Session["UserDetails"] as UserViewModel;
            string hphoto = PhotoManager.savePhoto(hasbandphoto, 0, "Benificiary");
            string wphoto = PhotoManager.savePhoto(wifephoto, 0, "Benificiary");

            beneficiaryViewModel.Hasband_Photo = hphoto; //PhotoManager.ConvertToBytes(hasbandphoto);
            beneficiaryViewModel.Wife_Photo = wphoto; //PhotoManager.ConvertToBytes(wifephoto);
            beneficiaryViewModel.ModifiedBy = user.UserName;
            beneficiaryViewModel.ModifiedDate = DateTime.Now;
            beneficiaryViewModel.CityId = user.CityId;

            var beniMaster = Mapper.BeneficiaryMapper.Attach(beneficiaryViewModel);

            _benificiaryService.Update(beniMaster);
            _benificiaryService.SaveChanges();

            ViewBag.Message = "sussess message";
            return RedirectToAction("Index");
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

        [HttpGet]
        [CustomAuthorize(Common.DataEntry, Common.Admin)]
        public ActionResult UploadBeneficiayExcel()
        {
            var cities = _cityMasterService.Get(r => r.IsActive == true);
            BeneficiaryViewModel um = new BeneficiaryViewModel();
            um.Cities = cities.Select(s => new SelectListItem { Text = s.CityName, Value = s.CityId.ToString() }).ToList();
            return View(um);
        }

        [HttpPost]
        [CustomAuthorize(Common.DataEntry, Common.Admin)]
        public ActionResult UploadBeneficiayExcel(HttpPostedFileBase postedFile, BeneficiaryViewModel beneficiaryViewModel)
        {
            var cities = _cityMasterService.Get(r => r.IsActive == true);
            BeneficiaryViewModel um = new BeneficiaryViewModel();
            um.Cities = cities.Select(s => new SelectListItem { Text = s.CityName, Value = s.CityId.ToString() }).ToList();

            if(postedFile == null)
            {
                ViewBag.Success = "postedFile is Null";
                return View(um);
            }

            try
            {
                int? cityId = beneficiaryViewModel.CityId;
                var userData = Session["UserDetails"] as UserViewModel;
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/UploadedExcels/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conStr = string.Empty;
                    switch (extension)
                    {
                        case ".xls":
                            conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx":
                            conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }

                    DataTable dt = new DataTable();
                    conStr = string.Format(conStr, filePath);

                    //Add data from excel to dt for validation
                    using (OleDbConnection conStrExcel = new OleDbConnection(conStr))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter daExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = conStrExcel;
                                conStrExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = conStrExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                conStrExcel.Close();

                                conStrExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                daExcel.SelectCommand = cmdExcel;
                                daExcel.Fill(dt);
                                conStrExcel.Close();
                            }
                        }
                    }

                    //Code for get only 6 digit of adhaar no and remove single quote from adhaar and DTR no
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var NewAdhaarNo = dt.Rows[i]["AdharNo"].ToString();
                        var NewDTRNo = dt.Rows[i]["DTRNo"].ToString();
                        var NewAccountNo = dt.Rows[i]["AccountNo"].ToString();
                        NewAdhaarNo = NewAdhaarNo.Substring(NewAdhaarNo.Length - 6);
                        NewDTRNo = NewDTRNo.Replace("'", "");
                        NewAccountNo = NewAccountNo.Replace("'", "");
                        dt.Rows[i]["AdharNo"] = NewAdhaarNo;
                        dt.Rows[i]["DTRNo"] = NewDTRNo;
                        dt.Rows[i]["AccountNo"] = NewAccountNo;
                    }
                    //dt.Columns["AdharNo"]=

                    //add new columns to existing datatable
                    dt.Columns.Add("CreatedDate", typeof(System.DateTime));
                    dt.Columns.Add("CreatedBy", typeof(System.String));
                    dt.Columns.Add("CompanyID", typeof(System.Int32));
                    dt.Columns.Add("Password", typeof(System.String));
                    dt.Columns.Add("CityId", typeof(System.Int32));

                    var password = "";
                    var beniFirstName = "";
                    var mobileNoFirstFour = "";
                    string error = "";

                    StringBuilder sb = new StringBuilder();

                    //validate data by looping before upload
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["CityId"].ToString() == "")
                        {
                            dt.Rows[i]["CityId"] = cityId;
                        }
                        if (dt.Rows[i]["CreatedDate"].ToString() == "")
                        {
                            dt.Rows[i]["CreatedDate"] = DateTime.Now;
                        }
                        if (dt.Rows[i]["CreatedBy"].ToString() == "")
                        {
                            dt.Rows[i]["CreatedBy"] = "System";
                        }
                        if (dt.Rows[i]["CompanyID"].ToString() == "")
                        {
                            dt.Rows[i]["CompanyID"] = userData.CompanyID;
                        }
                        if (dt.Rows[i]["BeneficairyName"].ToString() == "")
                        {
                            int RowNo = i + 2;
                            sb.AppendLine("Please enter Beneficiary Name in row " + RowNo);
                            error += "Please enter Beneficiary Name in row " + RowNo + Environment.NewLine;
                        }
                        else
                        {
                            beniFirstName = dt.Rows[i]["BeneficairyName"].ToString();
                            beniFirstName = !String.IsNullOrWhiteSpace(beniFirstName) && beniFirstName.Length >= 5 ? beniFirstName.Substring(0, 4) : beniFirstName;
                        }
                        if (dt.Rows[i]["FatherName"].ToString() == "")
                        {
                            int RowNo = i + 2;
                            sb.AppendLine("Please enter Father Name in row " + RowNo);
                            error += "Please enter Father Name in row " + RowNo + Environment.NewLine;
                        }
                        if (dt.Rows[i]["MobileNo"].ToString() == "")
                        {
                            var mobileno = dt.Rows[i]["MobileNo"].GetType();
                            int RowNo = i + 2;
                            sb.AppendLine("Please enter Mobile No in row " + RowNo);
                            error += "Please enter Mobile No in row " + RowNo + Environment.NewLine;
                        }
                        else
                        {
                            mobileNoFirstFour = dt.Rows[i]["MobileNo"].ToString();
                            mobileNoFirstFour = !String.IsNullOrWhiteSpace(mobileNoFirstFour) && mobileNoFirstFour.Length >= 5 ? mobileNoFirstFour.Substring(0, 4) : mobileNoFirstFour;
                        }

                        #region this Code commented and remain for if client want to add city id from excel data

                        //if (dt.Rows[i]["CityName"].ToString() == "")
                        //{
                        //    int RowNo = i + 2;
                        //    ViewBag.WrongBeniData = "Please enter City Name in row " + RowNo;
                        //    return View(um);
                        //}
                        //else
                        //{
                        //    dt.Rows[i]["CityName"] = cityId;
                        //}
                        //else
                        //{
                        //    var cityName = dt.Rows[i]["CityName"].ToString();
                        //    var cityId = 0;
                        //    cityId = _cityMasterService.Get().Where(c => c.CityName == cityName).Select(c => c.CityId).FirstOrDefault();
                        //    if(cityId != 0)
                        //    {
                        //        dt.Rows[i]["CityName"] = cityId;
                        //    }
                        //    else
                        //    {
                        //        int RowNo = i + 2;
                        //        ViewBag.WrongBeniData = "Invalid City Name in row " + RowNo; //City Name must be exact same as saved in database
                        //        return View(um);
                        //    }
                        //}
                        #endregion

                        if (dt.Rows[i]["DTRNo"].ToString() == "")
                        {
                            int RowNo = i + 2;
                            sb.AppendLine("Please enter DTR No in row " + RowNo);
                            error += "Please enter DTR No in row " + RowNo + Environment.NewLine;
                        }
                        //if (dt.Rows[i]["General"].ToString() == "")
                        //{
                        //    int RowNo = i + 2;
                        //    sb.AppendLine("Please enter Cast in row " + RowNo);
                        //    error += "Please enter Cast in row " + RowNo + Environment.NewLine;
                        //}
                        if (dt.Rows[i]["AdharNo"].ToString() == "")
                        {
                            int RowNo = i + 2;
                            sb.AppendLine("Please enter Adhar No in row " + RowNo);
                            error += "Please enter Adhar No in row " + RowNo + Environment.NewLine;
                        }
                        if (dt.Rows[i]["BankName"].ToString() == "")
                        {
                            int RowNo = i + 2;
                            sb.AppendLine("Please enter Bank Name in row " + RowNo);
                            error += "Please enter Bank Name in row " + RowNo + Environment.NewLine;
                        }
                        if (dt.Rows[i]["BranchName"].ToString() == "")
                        {
                            int RowNo = i + 2;
                            sb.AppendLine("Please enter Branch Name in row " + RowNo);
                            error += "Please enter Branch Name in row " + RowNo + Environment.NewLine;
                        }
                        if (dt.Rows[i]["IFSCCode"].ToString() == "")
                        {
                            int RowNo = i + 2;
                            sb.AppendLine("Please enter IFSC Code in row " + RowNo);
                            error += "Please enter IFSC Code in row " + RowNo + Environment.NewLine;
                        }
                        if (dt.Rows[i]["BankName"].ToString() == "")
                        {
                            int RowNo = i + 2;
                            sb.AppendLine("Please enter BankName in in row " + RowNo);
                            error += "Please enter BankName in in row " + RowNo + Environment.NewLine;
                        }
                        if (dt.Rows[i]["AccountNo"].ToString() == "")
                        {
                            int RowNo = i + 2;
                            sb.AppendLine("Please enter Account No in in row " + RowNo);
                            error += "Please enter Account No in in row " + RowNo + Environment.NewLine;
                        }
                        password = beniFirstName + mobileNoFirstFour;
                        if (dt.Rows[i]["Password"].ToString() == "")
                        {
                            dt.Rows[i]["Password"] = password;
                        }
                    }

                    //show blank data cells if exists in dt
                    if (!string.IsNullOrWhiteSpace(sb.ToString()))
                    {
                        ViewBag.WrongBeniData = sb.ToString().Replace(Environment.NewLine, "\n");
                        return View(um);
                    }

                    //indert data into DB from dt
                    conStr = ConfigurationManager.ConnectionStrings["ErrorLog"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        using (SqlTransaction sqlTransaction = con.BeginTransaction())
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conStr))
                            {
                                sqlBulkCopy.DestinationTableName = "BenificiaryMaster";

                                //Mapping with excel and sql
                                sqlBulkCopy.ColumnMappings.Add("BeneficairyName", "[BeneficairyName]");
                                sqlBulkCopy.ColumnMappings.Add("FatherName", "[FatherName]");
                                sqlBulkCopy.ColumnMappings.Add("Mother", "[Mother]");
                                sqlBulkCopy.ColumnMappings.Add("MobileNo", "[MobileNo]");
                                sqlBulkCopy.ColumnMappings.Add("PresentAddress", "[PresentAddress]");
                                sqlBulkCopy.ColumnMappings.Add("CityId", "[CityId]");
                                sqlBulkCopy.ColumnMappings.Add("DTRNo", "[DTRNo]");
                                sqlBulkCopy.ColumnMappings.Add("RecordNo", "[RecordNo]");
                                sqlBulkCopy.ColumnMappings.Add("Class", "[Class]");
                                sqlBulkCopy.ColumnMappings.Add("General", "[General]");
                                sqlBulkCopy.ColumnMappings.Add("Single", "[Single]");
                                sqlBulkCopy.ColumnMappings.Add("Disabled", "[Disabled]");
                                sqlBulkCopy.ColumnMappings.Add("Password", "[Password]");
                                sqlBulkCopy.ColumnMappings.Add("AdharNo", "[AdharNo]");
                                sqlBulkCopy.ColumnMappings.Add("VoterID", "[VoterID]");
                                sqlBulkCopy.ColumnMappings.Add("Area", "[Area]");
                                sqlBulkCopy.ColumnMappings.Add("MojaNo", "[MojaNo]");
                                sqlBulkCopy.ColumnMappings.Add("KhataNo", "[KhataNo]");
                                sqlBulkCopy.ColumnMappings.Add("KhasraNo", "[KhasraNo]");
                                sqlBulkCopy.ColumnMappings.Add("PlotNo", "[PlotNo]");
                                sqlBulkCopy.ColumnMappings.Add("PoliceStation", "[PoliceStation]");
                                sqlBulkCopy.ColumnMappings.Add("WardNo", "[WardNo]");
                                sqlBulkCopy.ColumnMappings.Add("District", "[District]");
                                sqlBulkCopy.ColumnMappings.Add("BankName", "[BankName]");
                                sqlBulkCopy.ColumnMappings.Add("BranchName", "[BranchName]");
                                sqlBulkCopy.ColumnMappings.Add("IFSCCode", "[IFSCCode]");
                                sqlBulkCopy.ColumnMappings.Add("AccountNo", "[AccountNo]");
                                sqlBulkCopy.ColumnMappings.Add("CreatedDate", "[CreatedDate]");
                                sqlBulkCopy.ColumnMappings.Add("CreatedBy", "[CreatedBy]");
                                //sqlBulkCopy.ColumnMappings.Add("ModifiedDate", "[ModifiedDate]");
                                //sqlBulkCopy.ColumnMappings.Add("ModifiedBy", "[ModifiedBy]");
                                sqlBulkCopy.ColumnMappings.Add("CompanyID", "[CompanyID]");
                                //sqlBulkCopy.ColumnMappings.Add("Wife_Photo", "[Wife_Photo]");
                                //sqlBulkCopy.ColumnMappings.Add("Hasband_Photo", "[Hasband_Photo]");

                                try
                                {
                                    sqlBulkCopy.WriteToServer(dt);
                                    sqlTransaction.Commit();
                                    ViewBag.Success = "Data Improted successfully";
                                }
                                catch (Exception ex)
                                {
                                    sqlTransaction.Rollback();
                                    ViewBag.WrongBeniData = "Kindly check the uploaded excel data, there is some proble with the format type or data";
                                    return View(um);
                                }
                            }
                        }
                        con.Close();
                    }
                }
                return View(um);
            }
            catch (Exception ex)
            {
                ViewBag.WrongBeniData = ex.Message;
                return View(um);
            }
        }
    }
}
