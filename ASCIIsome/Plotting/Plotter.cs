using System;
using System.Drawing;

namespace ASCIIsome.Plotting
{
    public static class Plotter
    {
        // TODO: [HV] Build ConfigChanged event and relative types

        private static ViewModel currentViewModel;
#pragma warning disable 169
        private static Bitmap originalBitmap;
        private static Bitmap resizedBitmap;
        private static Range<double> grayscaleRange;
        private static double[][] grayscaleIndexArray;
        private static char[][] characterMatchedArray;
#pragma warning restore 169

        private static void OnConfigChanged(ViewModel inputViewModel, string configPropertyName) // [HV] Is the subscriber of ConfigChanged event
        {
            currentViewModel = inputViewModel;
            switch (configPropertyName)
            {
                case nameof(ViewModel.ImgSourcePath):
                    Plot(PlottingStage.AccessImageFromFilePath);
                    break;

                case nameof(ViewModel.IsAspectRatioKept): // TODO: [HV] Need to do more on this case regarding CharImgHeight and CharImgWidth properties and their corresponding UI controls
                case nameof(ViewModel.CharImgHeight):
                case nameof(ViewModel.CharImgWidth):
                    Plot(PlottingStage.ResizeImage);
                    break;

                case nameof(ViewModel.IsDynamicGrayscaleRangeEnabled): // TODO: [HV] Use when clause for basic conditional decision when necessary
                    Plot(PlottingStage.CalculateGrayscaleRange);
                    break;

                case nameof(ViewModel.IsGrayscaleRangeInverted):
                    Plot(PlottingStage.InvertGrayscaleIndex);
                    break;

                case nameof(ViewModel.CurrentCharSet):
                    Plot(PlottingStage.MatchCharacter);
                    break;
            }
        }

        private static void Plot(PlottingStage startingStage)
        {
            switch (startingStage)
            {
                case PlottingStage.AccessImageFromFilePath:
                    AccessImageFromFilePath();
                    goto case PlottingStage.ResizeImage;

                case PlottingStage.ResizeImage:
                    ResizeImage();
                    goto case PlottingStage.CalculateGrayscaleRange;

                case PlottingStage.CalculateGrayscaleRange:
                    CalculateGrayscaleRange();
                    goto case PlottingStage.AssignGrayscaleIndex; // TODO: [HV] Use if/else or when clause checking to decide whether goto 0x30 or 0x31

                case PlottingStage.AssignGrayscaleIndex:
                    AssignGrayscaleIndex();
                    goto case PlottingStage.MatchCharacter;

                case PlottingStage.InvertGrayscaleIndex:
                    InvertGrayscaleIndex();
                    goto case PlottingStage.MatchCharacter;

                case PlottingStage.MatchCharacter:
                    MatchCharacter();
                    goto case PlottingStage.OutputCharacter;

                case PlottingStage.OutputCharacter:
                    OutputCharacter();
                    break;
            }
        }

        private static void OutputCharacter() => throw new NotImplementedException();
        private static void MatchCharacter() => throw new NotImplementedException();
        private static void InvertGrayscaleIndex() => throw new NotImplementedException();
        private static void AssignGrayscaleIndex() => throw new NotImplementedException();
        private static void CalculateGrayscaleRange() => throw new NotImplementedException();
        private static void ResizeImage() => throw new NotImplementedException();
        private static void AccessImageFromFilePath() => throw new NotImplementedException();

        public static void OutputEnumerateConfig(ViewModel inputViewModel) => inputViewModel.CharOut =
            "CharImgHeight: " + inputViewModel.CharImgHeight + Environment.NewLine +
            "CharImgWidth: " + inputViewModel.CharImgWidth + Environment.NewLine +
            "ImgSource: " + (inputViewModel.ImgSourcePath ?? "(null)") + Environment.NewLine +
            "IsAspectRatioKept: " + inputViewModel.IsAspectRatioKept + Environment.NewLine +
            "IsDynamicGrayscaleRangeEnabled: " + inputViewModel.IsDynamicGrayscaleRangeEnabled + Environment.NewLine +
            "IsGrayscaleRangeInverted: " + inputViewModel.IsGrayscaleRangeInverted + Environment.NewLine +
            "CurrentCharSet: " + inputViewModel.CurrentCharSet + Environment.NewLine;
    }
}
