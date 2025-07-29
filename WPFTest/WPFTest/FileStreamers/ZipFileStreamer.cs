using Microsoft.Win32;
using System.IO;

namespace WPFTest.FileStreamers
{
    public static class ZipFileStreamer 
    {
        public static byte[]? SetFile()
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Zip files(*.zip)|*.zip"
            };

            ofd.ShowDialog();

            if (ofd.CheckFileExists != true) return null;

            var path = ofd.FileName;

            if (path == string.Empty) return null;

            var bytes = File.ReadAllBytes(path);

            return bytes;
        }

        public static void GetFile(byte[] bytes)
        {
            var sfd = new SaveFileDialog
            {
                FileName = "exercise",
                Filter = "ZIP архивы (*.zip)|*.zip|Все файлы (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".zip",
                AddExtension = true,
                OverwritePrompt = true,
                CheckPathExists = true
            };

            sfd.ShowDialog();

            if (sfd.CheckPathExists != true) return;

            var path = sfd.FileName;

            if (path == string.Empty) return;

            File.WriteAllBytes(path, bytes);
        }
    }
}
