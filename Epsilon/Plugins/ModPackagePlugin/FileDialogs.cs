using Microsoft.Win32;
using System.IO;

namespace ModPackagePlugin
{
    static class FileDialogs
    {
        public static bool RunBrowseCacheDialog(out FileInfo chosenFile)
        {
            chosenFile = null;

            var ofd = new OpenFileDialog()
            {
                Title = "Open base cache",
                FileName = "tags.dat",
            };
            if (ofd.ShowDialog() == false)
                return false;

            chosenFile = new FileInfo(ofd.FileName);
            return true;
        }

        public static bool RunSaveDialog(out FileInfo chosenFile, string initialDirectory = null)
        {
            chosenFile = null;

            var ofd = new SaveFileDialog()
            {
                InitialDirectory = initialDirectory,
                Title = "Save",
                FileName = "untitled.pak",
                Filter = "Mod Package (*.pak)|*.pak",
            };
            if (ofd.ShowDialog() == false)
                return false;

            chosenFile = new FileInfo(ofd.FileName);
            return true;
        }
    }
}
