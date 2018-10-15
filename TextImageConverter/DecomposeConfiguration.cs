using System.Drawing;

namespace TextImageConverter
{
    public class DecomposeConfiguration : ConfigurationBase
    {
        public Bitmap Bitmap { get; set; } // [HV] automatically generated depending on SourcePath
        public int ImgWidth { get; set; } // [HV] automatically calculated depending on Bitmap
        public int ImgHeight { get; set; } // [HV] automatically calculated depending on Bitmap
    }
}
