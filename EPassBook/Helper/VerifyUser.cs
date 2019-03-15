using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace EPassBook.Helper
{
    public class VerifyUser
    {
        public string user { get; set; }
        public string password { get; set; }
        public string msisdn { get; set; }
        public string sid { get; set; }
        public bool fl { get; set; }
        public int gwid { get; set; }
        public string OTP { get; set; }

        string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

        public string SendOtp()
        {
            sid = System.Configuration.ConfigurationManager.AppSettings["sid"];
            user = System.Configuration.ConfigurationManager.AppSettings["user"];
            password = System.Configuration.ConfigurationManager.AppSettings["password"];

            string response = string.Empty;
            //string otp = GenerateRandomOTP(4);
            //string urlPromotional = "http://sms.sminfomedia.in/vendorsms/pushsms.aspx?user="+ user + "&password=" + password + "&msisdn=" + msisdn + "&sid=" + sid + "&msg=" + OTP + "&fl=0 ";
            string urlTransactional = "http://sms.sminfomedia.in/vendorsms/pushsms.aspx?user=" + user + "&password=" + password + "&msisdn=" + msisdn + "&sid=" + sid + "&msg=" + OTP + "&fl=0&gwid=2 ";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(urlTransactional);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            
            if ( myHttpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                response = "OTP Sent";
            }
            else
            {
                response = "Some error occured please try again";
            }
            return response;
        }
        public string GenerateRandomOTP(int iOTPLength)
        {
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }
    }
}