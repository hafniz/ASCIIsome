namespace ASCIIsome
{
    public enum PlottingStage
    {
        AccessImageFromFilePath = 0x00,
        ResizeImage = 0x10,
        CalculateGrayscaleRange = 0x20,
        AssignGrayscaleIndex = 0x30,
        InvertGrayscaleIndex = 0x31,
        MatchCharacter = 0x40,
        OutputCharacter = 0x50
    }
}
