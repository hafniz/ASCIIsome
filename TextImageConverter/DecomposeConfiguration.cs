using System.Drawing;

namespace TextImageConverter
{
    public class DecomposeConfiguration : ConfigurationBase
    {
        public Bitmap Bitmap { get; set; } // automatically generated depending on SourcePath
        public int ImgWidth { get; set; } // automatically calculated depending on Bitmap
        public int ImgHeight { get; set; } // automatically calculated depending on Bitmap
    }
}
