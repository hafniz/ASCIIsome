using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using static System.Console;

namespace TextImageConverter
{
    public static class Composer
    {
        public static void Compose()
        {
            ComposeConfiguration currentConfig = new ComposeConfiguration();
            GetFilePath(currentConfig);
            using (FileStream fileStream = new FileStream(currentConfig.WorkingPath, FileMode.Open))
            {
                currentConfig.FileLength = fileStream.Length;
                WriteLine($"The size of this file is {currentConfig.FileLength} bytes. ");
                GetOffsetGenerator(currentConfig);
                WriteOffset(currentConfig, fileStream);
                ProcessFileTail(currentConfig, fileStream);
                GetImageSize(currentConfig);
                Bitmap bitmap = new Bitmap(currentConfig.ImgWidth.Value, currentConfig.ImgHeight.Value);
                Stopwatch stopWatch = Stopwatch.StartNew();
                stopWatch.Start();
                WriteLine("Started generating image... ");
                PlotPixels(currentConfig, fileStream, bitmap);
                stopWatch.Stop();
                WriteLine($"Process completed in {stopWatch.Elapsed}. ");
                SaveImage(bitmap, currentConfig);
            }
            File.Delete(currentConfig.WorkingPath); // [HV] However, if the user terminates the program before it finishes by itself, the temp file will not be deleted and thus disk space will be unnecessarily occupied in a relatively long term
            WriteLine("Done. ");
        }

        public static void SilentCompose(ComposeConfiguration currentConfig)
        {
            currentConfig.WorkingPath = Path.GetTempFileName();
            File.Copy(currentConfig.SourcePath, currentConfig.WorkingPath, true);
            using (FileStream fileStream = new FileStream(currentConfig.WorkingPath, FileMode.Open))
            {
                currentConfig.FileLength = fileStream.Length;
                WriteOffset(currentConfig, fileStream);
                ProcessFileTail(currentConfig, fileStream);
                CalculateImageSize(currentConfig);
                Bitmap bitmap = new Bitmap(currentConfig.ImgWidth.Value, currentConfig.ImgHeight.Value);
                PlotPixels(currentConfig, fileStream, bitmap);
                bitmap.Save(currentConfig.SavePath);
            }
            File.Delete(currentConfig.WorkingPath);
        }

        private static void WriteOffset(ComposeConfiguration currentConfig, FileStream fileStream)
        {
            if (currentConfig.OffsetGenerator != null)
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < currentConfig.FileLength; i++)
                {
                    int originalByte = fileStream.ReadByte();
                    byte byteWithOffset = (byte)((originalByte + currentConfig.OffsetGenerator.Next(256)) % 256); // [HV] Expectation: ((0 + 0) % 256) = 0 to ((255 + 255) % 256) = 254
                    fileStream.Seek(-1, SeekOrigin.Current);
                    fileStream.WriteByte(byteWithOffset);
                }
            }
        }

        private static void GetOffsetGenerator(ComposeConfiguration currentConfig)
        {
            WriteLine("You may specify an integer as seed to encrypt the image if you would like to, otherwise, press Enter to continue. Please note that you will need to enter the same seed when reading the image generated to correctly decompose it into a file. ");
            while (true)
            {
                try
                {
                    string inputSeed = ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputSeed))
                    {
                        currentConfig.OffsetSeed = (int)long.Parse(inputSeed);
                        currentConfig.OffsetGenerator = new Random(currentConfig.OffsetSeed.Value);
                    }
                    break;
                }
                catch (Exception e)
                {
                    WriteLine($"Error: {e.Message} Please check your input and try again: ");
                }
            }
        }

        private static void SaveImage(Bitmap bitmap, ComposeConfiguration currentConfig)
        {
            WriteLine("Please specify the path for the image to be saved: ");
            while (true)
            {
                try
                {
                    currentConfig.SavePath = Path.GetFullPath(ReadLine());
                    bitmap.Save(currentConfig.SavePath);
                    WriteLine("Image saved. ");
                    break;
                }
                catch (Exception e)
                {
                    WriteLine($"Error: {e.Message} Please check your input and try again: ");
                }
            }
        }

        private static Color GetCurrentColor(long currentPosition, FileStream fileStream, ComposeConfiguration currentConfig)
        {
            if (currentPosition < currentConfig.FileLength)
            {
                fileStream.Position = currentPosition;
                int red = fileStream.ReadByte();
                int green = fileStream.ReadByte();
                int blue = fileStream.ReadByte();
                return Color.FromArgb(red, green, blue);
            }
            return Color.Black;
        }

        private static void PlotPixels(ComposeConfiguration currentConfig, FileStream fileStream, Bitmap bitmap)
        {
            long currentPosition = 0;
            for (int y = 0; y < currentConfig.ImgHeight.Value; y++)
            {
                for (int x = 0; x < currentConfig.ImgWidth.Value; x++)
                {
                    Color currentColor = GetCurrentColor(currentPosition, fileStream, currentConfig);
                    bitmap.SetPixel(x, y, currentColor);
                    currentPosition += 3;
                }
            }
        }

        private static void GetImageSize(ComposeConfiguration currentConfig)
        {
            WriteLine("By default, an image with width and height as consistent as possible will be generated. However, you may specify the width, height, or both dimensions of the image. ");
            while (true)
            {
                WriteLine("Please specify the width of the image if you would like to, otherwise, press Enter to continue. ");
                try
                {
                    string inputWidth = ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputWidth))
                    {
                        currentConfig.ImgWidth = int.Parse(inputWidth);
                    }
                    break;
                }
                catch (Exception e)
                {
                    WriteLine($"Error: {e.Message} Please check your input and try again: ");
                }
            }

            WriteLine("Please specify the height of the image if you would like to, otherwise, press Enter to continue. ");
            while (true)
            {
                try
                {
                    string inputHeight = ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputHeight))
                    {
                        currentConfig.ImgHeight = int.Parse(inputHeight);
                    }
                    break;
                }
                catch (Exception e)
                {
                    WriteLine($"Error: {e.Message} Please check your input and try again: ");
                }
            }

            long pixelCount = currentConfig.FileLength / 3;
            if (currentConfig.ImgHeight.HasValue && currentConfig.ImgWidth.HasValue)
            {
                if (currentConfig.ImgHeight * currentConfig.ImgWidth > pixelCount)
                {
                    WriteLine("Warning: Your specified size of image has a larger capability than the size of the file. Please note that the remaining pixels will be filled with black color. ");
                }
                else if (currentConfig.ImgHeight * currentConfig.ImgWidth < pixelCount)
                {
                    WriteLine("Warning: Your specified size of image cannot fully contain the information stored in the file. Please note that the part of the file beyond the capability of the image will be truncated. ");
                }
            }
            else
            {
                CalculateImageSize(currentConfig);
                WriteLine($"An image of {currentConfig.ImgWidth} pixels in width and {currentConfig.ImgHeight} pixels in height will be generated. ");
            }
        }

        private static void CalculateImageSize(ComposeConfiguration currentConfig)
        {
            double pixelCount = currentConfig.FileLength / 3; // [HV] currentConfig.FileLength can be surely divided by 3 with no remainder, however pixelCount need to be in a non-integral type to prevent force flooring during division operation with an int
            if (currentConfig.ImgHeight.HasValue && !currentConfig.ImgWidth.HasValue)
            {
                double imgWidth = pixelCount / currentConfig.ImgHeight.Value;
                currentConfig.ImgWidth = (int)Math.Ceiling(imgWidth);
            }
            else if (!currentConfig.ImgHeight.HasValue && currentConfig.ImgWidth.HasValue)
            {
                double imgHeight = pixelCount / currentConfig.ImgWidth.Value;
                currentConfig.ImgHeight = (int)Math.Ceiling(imgHeight);
            }
            else if (!currentConfig.ImgHeight.HasValue && !currentConfig.ImgWidth.HasValue)
            {
                currentConfig.ImgHeight = (int)Math.Ceiling(Math.Sqrt(pixelCount));
                double imgWidth = pixelCount / currentConfig.ImgHeight.Value;
                currentConfig.ImgWidth = (int)Math.Ceiling(imgWidth);
            }
        }

        private static void ProcessFileTail(ComposeConfiguration currentConfig, FileStream fileStream)
        {
            fileStream.Seek(0, SeekOrigin.End);
            fileStream.WriteByte(23); // [HV] Append 'End of Transmission Block' byte at the end of the file
            switch ((currentConfig.FileLength + 1) % 3) // [HV] Ensure the length (in bytes) is divisible by 3, which is the number of bytes that a RGB24 pixel may contain
            {
                case 1:
                    fileStream.WriteByte(0);
                    fileStream.WriteByte(0);
                    break;
                case 2:
                    fileStream.WriteByte(0);
                    break;
            }
            currentConfig.FileLength = fileStream.Length;
        }

        private static void GetFilePath(ComposeConfiguration currentConfig)
        {
            WriteLine("Please specify the path for the file to be read: ");
            while (true)
            {
                try
                {
                    currentConfig.SourcePath = Path.GetFullPath(ReadLine());
                    currentConfig.WorkingPath = Path.GetTempFileName();
                    File.Copy(currentConfig.SourcePath, currentConfig.WorkingPath, true);
                    break;
                }
                catch (Exception e)
                {
                    WriteLine($"Error: {e.Message} Please check your input and try again: ");
                }
            }
        }
    }
}
