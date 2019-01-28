using System.Drawing;

#nullable enable
namespace TextImageConverter
{
    public class DecomposeConfiguration : ConfigurationBase
    {
        public Bitmap Bitmap { get; set; } // [HV] Automatically generated depending on SourcePath
        public int ImgWidth { get; set; } // [HV] Automatically calculated depending on Bitmap
        public int ImgHeight { get; set; } // [HV] Automatically calculated depending on Bitmap
    }
}
