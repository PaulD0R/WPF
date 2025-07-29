using Microsoft.Win32;
using System.IO;

namespace WPFTest.FileStreamers
{
    public static class ImgFileStreamer
    {
        private static readonly string[] SupportedImageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff" };

        public static byte[]? SetFile()
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff|All Files|*.*",
                Title = "Select an Image File"
            };

            if (ofd.ShowDialog() != true) return null;

            if (!ofd.CheckFileExists || string.IsNullOrEmpty(ofd.FileName)) return null;

            var fileExt = Path.GetExtension(ofd.FileName).ToLower();
            if (!SupportedImageExtensions.Contains(fileExt))
            {
                return null;
            }

            try
            {
                return File.ReadAllBytes(ofd.FileName);
            }
            catch
            {
                return null;
            }
        }

        public static bool GetFile(byte[] imageBytes, string defaultFileName = "image")
        {
            var sfd = new SaveFileDialog
            {
                FileName = defaultFileName,
                Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp|GIF Image|*.gif|TIFF Image|*.tiff|All Files|*.*",
                FilterIndex = 2,
                DefaultExt = ".png",
                AddExtension = true,
                OverwritePrompt = true,
                Title = "Save Image File"
            };

            if (sfd.ShowDialog() != true) return false;

            try
            {
                File.WriteAllBytes(sfd.FileName, imageBytes);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
