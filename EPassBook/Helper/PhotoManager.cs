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
    }
}