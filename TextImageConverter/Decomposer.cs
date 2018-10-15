using System;
using static System.Console;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace TextImageConverter
{
    public static class Decomposer
    {
        public static void Decompose()
        {
            DecomposeConfiguration currentConfig = new DecomposeConfiguration();
            GetFilePath(currentConfig);
            WriteLine($"The image is of {currentConfig.ImgWidth} pixels in width and {currentConfig.ImgHeight} pixels in height. The pixels contain data of {currentConfig.ImgHeight * currentConfig.ImgWidth * 3} bytes. ");
            currentConfig.WorkingPath = Path.GetTempFileName();
            GetOffsetGenerator(currentConfig);
            using (FileStream fileStream = new FileStream(currentConfig.WorkingPath, FileMode.Open))
            {
                Stopwatch stopWatch = Stopwatch.StartNew();
                stopWatch.Start();
                WriteLine("Started reading pixels... ");
                ReadPixels(currentConfig, fileStream);
                ProcessFileTail(fileStream);
                RemoveOffset(currentConfig, fileStream);
                stopWatch.Stop();
                WriteLine($"Process completed in {stopWatch.Elapsed}. ");
            }
            SaveFile(currentConfig);
            File.Delete(currentConfig.WorkingPath);
            WriteLine("Done. ");
        }

        public static void SilentDecompose(DecomposeConfiguration currentConfig)
        {
            currentConfig.Bitmap = new Bitmap(currentConfig.SourcePath);
            currentConfig.WorkingPath = Path.GetTempFileName();
            currentConfig.ImgHeight = currentConfig.Bitmap.Height;
            currentConfig.ImgWidth = currentConfig.Bitmap.Width;
            using (FileStream fileStream = new FileStream(currentConfig.WorkingPath, FileMode.Open))
            {
                ReadPixels(currentConfig, fileStream);
                ProcessFileTail(fileStream);
                RemoveOffset(currentConfig, fileStream);
            }
            File.Copy(currentConfig.WorkingPath, currentConfig.SavePath, true);
            File.Delete(currentConfig.WorkingPath);
        }

        private static void RemoveOffset(DecomposeConfiguration currentConfig, FileStream fileStream)
        {
            if (currentConfig.OffsetGenerator != null)
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < fileStream.Length; i++)
                {
                    int byteWithOffset = fileStream.ReadByte();
                    int originalByte = (byteWithOffset - currentConfig.OffsetGenerator.Next(256)) % 256;
                    byte originalByteCalibrated = (byte)(originalByte >= 0 ? originalByte : originalByte + 256);
                    fileStream.Seek(-1, SeekOrigin.Current);
                    fileStream.WriteByte(originalByteCalibrated);
                }
            }
        }

        private static void GetOffsetGenerator(DecomposeConfiguration currentConfig)
        {
            WriteLine("You may specify an integer as seed to decrypt the image if you would like to, otherwise, press Enter to continue. ");
            while (true)
            {
                try
                {
                    string inputSeed = ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputSeed))
                    {
                        currentConfig.OffsetSeed = int.Parse(inputSeed);
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

        private static void SaveFile(DecomposeConfiguration currentConfig)
        {
            WriteLine("Please specify the path for the file to be saved: ");
            while (true)
            {
                try
                {
                    currentConfig.SavePath = Path.GetFullPath(ReadLine());
                    File.Copy(currentConfig.WorkingPath, currentConfig.SavePath, true);
                    WriteLine("File saved. ");
                    break;
                }
                catch (Exception e)
                {
                    WriteLine($"Error: {e.Message} Please check your input and try again: ");
                }
            }
        }

        private static void ProcessFileTail(FileStream fileStream)
        {
            while (true)
            {
                fileStream.Seek(-1, SeekOrigin.End);
                if (fileStream.ReadByte() == 0) // [HV] Read the last byte of the file. 
                {
                    fileStream.SetLength(fileStream.Length - 1);
                }
                else
                {
                    fileStream.SetLength(fileStream.Length - 1); // [HV] Also delete the 'End of Transmission Block' byte appended during composing process. 
                    break;
                }
            }
        }

        private static void ReadPixels(DecomposeConfiguration currentConfig, FileStream fileStream)
        {
            for (int y = 0; y < currentConfig.ImgHeight; y++)
            {
                for (int x = 0; x < currentConfig.ImgWidth; x++)
                {
                    byte[] currentBytes = GetBytes(x, y, currentConfig.Bitmap);
                    WriteBytes(fileStream, currentBytes);
                }
            }
        }

        private static void WriteBytes(FileStream fileStream, byte[] currentBytes)
        {
            fileStream.Seek(0, SeekOrigin.End);
            fileStream.Write(currentBytes, 0, 3);
        }

        private static byte[] GetBytes(int x, int y, Bitmap bitmap)
        {
            Color currentColor = bitmap.GetPixel(x, y);
            byte[] bytes = new byte[3];
            bytes[0] = currentColor.R;
            bytes[1] = currentColor.G;
            bytes[2] = currentColor.B;
            return bytes;
        }

        private static void GetFilePath(DecomposeConfiguration currentConfig)
        {
            WriteLine("Please specify the path for the image to be read: ");
            while (true)
            {
                try
                {
                    currentConfig.SourcePath = Path.GetFullPath(ReadLine());
                    currentConfig.Bitmap = new Bitmap(currentConfig.SourcePath);
                    currentConfig.ImgHeight = currentConfig.Bitmap.Height;
                    currentConfig.ImgWidth = currentConfig.Bitmap.Width;
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
