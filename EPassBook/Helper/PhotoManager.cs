using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EPassBook.Helper
{
    public class PhotoManager
    {
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;

            BinaryReader reader = new BinaryReader(image.InputStream);

            imageBytes = reader.ReadBytes((int)image.ContentLength);

            return imageBytes;

        }
    }
}