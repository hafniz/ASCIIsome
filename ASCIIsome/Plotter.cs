using System;

namespace ASCIIsome
{
    public static class Plotter
    {
        public static void Plot(ViewModel currentConfig)
        {
            DebugEnumerateConfig(currentConfig);
            if (currentConfig.ImgSource != null)
            {
                // TODO: [HV] (Re)plot the char graph
            }
        }

        private static void DebugEnumerateConfig(ViewModel currentConfig) => currentConfig.CharOut = "CharImgHeight: " + currentConfig.CharImgHeight + Environment.NewLine +
            "CharImgWidth: " + currentConfig.CharImgWidth + Environment.NewLine +
            "ImgSource: " + currentConfig.ImgSource + Environment.NewLine +
            "IsAspectRatioKept: " + currentConfig.IsAspectRatioKept + Environment.NewLine +
            "IsDynamicGrayscaleRangeEnabled: " + currentConfig.IsDynamicGrayscaleRangeEnabled + Environment.NewLine +
            "IsGrayscaleRangeInverted: " + currentConfig.IsGrayscaleRangeInverted + Environment.NewLine +
            "CurrentCharSet: " + currentConfig.CurrentCharSet;
    }
}
