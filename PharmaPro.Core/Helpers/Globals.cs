using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Helpers
{
    public static class Globals
    {
        public static String StorageRootPath
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return @"\www";
                return Directory.GetCurrentDirectory();
            }
        }
        public static String UploadPath { get => "Uploads"; }
        public static String UploadLogo { get => "Logo"; }
        public static String StaticImagePath { get => "Images"; }
    }
}
