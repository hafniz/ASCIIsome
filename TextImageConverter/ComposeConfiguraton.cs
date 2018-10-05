namespace TextImageConverter
{
    public class ComposeConfiguration : ConfigurationBase
    {
        public long FileLength { get; set; } // automatically calculated depending on SourcePath
        public int? ImgWidth { get; set; } // optionally spcified by user
        public int? ImgHeight { get; set; } // optionally specified by user
    }
}
