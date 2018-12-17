using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ASCIIsome.Commands
{
    public sealed class ImportFromClipboardCommand : CommonCommandBase
    {
        public ImportFromClipboardCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) // TODO: [HV] Support Ctrl-V shortcut and drag-and-drop to window
        {
            if (Clipboard.ContainsData(DataFormats.Bitmap))
            {
                string tempPath = Path.GetTempFileName();
                BitmapSource bitmapSource = (BitmapSource)Clipboard.GetData(DataFormats.Bitmap);
                SaveBitmapSourceToFile(bitmapSource, tempPath);
                CurrentViewModel.ImgSourcePath = tempPath;
                CurrentViewModel.StatusBarText = Properties.Resources.ImportedFromClipboard;
            }
            else if (Clipboard.ContainsFileDropList())
            {
                StringCollection fileNames = Clipboard.GetFileDropList();
                foreach (string fileName in fileNames)
                {
                    string fileExtension = fileName.Remove(0, fileName.LastIndexOf('.'));
                    if (IsOfSupportedType(fileExtension))
                    {
                        CurrentViewModel.ImgSourcePath = fileName; // WARNING: [HV] Only the lastly single-selected file of supported type or first file of supported type shown in the file explorer in rectangular selection will be opened
                        CurrentViewModel.StatusBarText = Properties.Resources.ImportedFromClipboard;
                        return;
                    }
                }
                CurrentViewModel.StatusBarText = Properties.Resources.UnsupportedType;
            }
            else if (Clipboard.ContainsText())
            {
                FileInfo fileInfo;
                try
                {
                    fileInfo = new FileInfo(Clipboard.GetText());
                }
                catch (Exception)
                {
                    CurrentViewModel.StatusBarText = Properties.Resources.InvalidFilePath;
                    return;
                }

                if (IsOfSupportedType(fileInfo.Extension))
                {
                    CurrentViewModel.ImgSourcePath = $"{fileInfo.DirectoryName}\\{fileInfo.Name}";
                    CurrentViewModel.StatusBarText = Properties.Resources.ImportedFromClipboard;
                }
                else
                {
                    CurrentViewModel.StatusBarText = Properties.Resources.UnsupportedType; // TODO: [HV] Call TextImageConverter on encountering unsupported file type
                }
            }
            else
            {
                CurrentViewModel.StatusBarText = Properties.Resources.UnsupportedType;
            }
        }

        private static void SaveBitmapSourceToFile(BitmapSource bitmapSource, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder bitmapEncoder = new PngBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                bitmapEncoder.Save(fileStream);
            }
        }

        private static bool IsOfSupportedType(string fileExtension)
        {
            List<string> supportedFileExtensions = new List<string>
            {
                ".bmp",
                ".jpg",
                ".jpeg",
                ".jxr",
                ".png",
                ".tif",
                ".tiff",
                ".gif",
                ".ico"
            };

            foreach (string extension in supportedFileExtensions)
            {
                if (fileExtension == extension)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
