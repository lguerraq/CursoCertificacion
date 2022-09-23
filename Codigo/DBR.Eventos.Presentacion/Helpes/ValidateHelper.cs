using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace TGS.GISSAT.Web.Helpes
{
    public static class ValidateHelper
    {
        public static bool isPDF(HttpPostedFileBase file)
        {
            if (Path.GetExtension(file.FileName).ToLower().EndsWith("pdf") && IsPDFHeader(file.InputStream))
            {
                return true;
            }

            return false;
        }
        public static bool isExcel(HttpPostedFileBase file)
        {
            if (Path.GetExtension(file.FileName).ToLower().EndsWith("xlsx") || Path.GetExtension(file.FileName).ToLower().EndsWith("xls"))
            {
                return true;
            }

            return false;
        }
        public static bool isWord(HttpPostedFileBase file)
        {
            if (Path.GetExtension(file.FileName).ToLower().EndsWith("docx"))
            {
                return true;
            }

            return false;
        }
        public static bool isZip(HttpPostedFileBase file)
        {
            if (Path.GetExtension(file.FileName).ToLower().EndsWith("zip"))
            {
                return true;
            }

            return false;
        }
        public static bool isImage(HttpPostedFileBase file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowedFiles = new List<string>() { ".jpg", ".jpeg", ".png" };
            if (allowedFiles.Contains(extension) && HasJpegHeader(file.InputStream))
            {
                return true;
            }

            return false;
        }
        private static bool IsPDFHeader(Stream file)
        {
            MemoryStream stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;
            byte[] buffer = null;
            //FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(stream);
            //long numBytes = new FileInfo(fileName).Length;
            //buffer = br.ReadBytes((int)numBytes);
            buffer = br.ReadBytes(5);

            var enc = new ASCIIEncoding();
            var header = enc.GetString(buffer);

            //%PDF−1.0
            // If you are loading it into a long, this is (0x04034b50).
            if (buffer[0] == 0x25 && buffer[1] == 0x50
                && buffer[2] == 0x44 && buffer[3] == 0x46)
            {
                return header.StartsWith("%PDF-");
            }

            return false;
        }
        private static bool HasJpegHeader(Stream file)
        {
            MemoryStream stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;
            using (BinaryReader br = new BinaryReader(stream))
            {
                UInt16 soi = br.ReadUInt16();  // Start of Image (SOI) marker (FFD8)
                UInt16 marker = br.ReadUInt16(); // JFIF marker (FFE0) or EXIF marker(FF01)
                return (soi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff) || (soi == 0x5089 && marker == 0x474e);
            }
        }
    }
}