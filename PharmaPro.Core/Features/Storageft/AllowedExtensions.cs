namespace PharmaPro.Core.Features.Storageft
{
    public static class AllowedExtensions
    {
        public static List<String> Get() => new List<string>()
        {
            "JPEG",
            "JPG",
            "PNG",
            "GIF",
            "BMP",
            "TIFF",
            "TIF",
            "RAW",
            "EPS",
            "TGA",
            "PICT"
        };
    }
}
