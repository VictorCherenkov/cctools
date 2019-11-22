using System;
using System.IO;
using System.IO.Compression;

namespace CCTools.Dll.AttachedLogic
{
    public static class ZipArchiveExtensions
    {
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (var file in archive.Entries)
            {
                try
                {
                    var completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                    var directory = Path.GetDirectoryName(completeFileName);

                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    if (file.Name != string.Empty)
                        file.ExtractToFile(completeFileName, true);
                }
                catch
                {
                }
            }
        }
    }
}