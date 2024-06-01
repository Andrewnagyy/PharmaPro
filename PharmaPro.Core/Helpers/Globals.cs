using System.Runtime.InteropServices;

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
