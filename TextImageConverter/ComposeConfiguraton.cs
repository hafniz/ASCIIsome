namespace TextImageConverter
{
    public class ComposeConfiguration : ConfigurationBase
    {
        public long FileLength { get; set; } // [HV] automatically calculated depending on SourcePath
        public int? ImgWidth { get; set; } // [HV] optionally spcified by user
        public int? ImgHeight { get; set; } // [HV] optionally specified by user
    }
}
