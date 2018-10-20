namespace TextImageConverter
{
    public class ComposeConfiguration : ConfigurationBase
    {
        public long FileLength { get; set; } // [HV] Automatically calculated depending on SourcePath
        public int? ImgWidth { get; set; } // [HV] Optionally specified by user
        public int? ImgHeight { get; set; } // [HV] Optionally specified by user
    }
}
