using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EPassBook.Helper
{
    public static class PhotoManager
    {
        public static byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            if (image != null)
            {
                BinaryReader reader = new BinaryReader(image.InputStream);

                imageBytes = reader.ReadBytes((int)image.ContentLength);
            }
            return imageBytes;

        }

        public static string savePhoto(HttpPostedFileBase hp, int installmentid, string flag)
        {
            if (hp != null)
            {
                var allowedExtensions = new[] { ".png", ".jpg", "jpeg" };
                var fileName = Path.GetFileName(hp.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(hp.FileName); //getting the extension(ex-.jpg)  

                if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                {
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name + "_" + installmentid.ToString() + ext; //appending the name with id  
                    var path = "";
                    // store the file inside ~/project folder(Img)  
                    if (flag == "SiteEngineer")
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/SiteEngPhotos"), myfile);
                    }
                    else if(flag == "Benificiary")
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/BeneficiaryImages"), myfile);
                    }
                    hp.SaveAs(path);
                    return myfile;
                }
            }
            else
            {
                return "empty";
            }
            return "fail";
        }
    }
}