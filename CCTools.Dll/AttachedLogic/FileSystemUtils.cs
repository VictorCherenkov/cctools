using System;
using System.IO;
using System.Linq;

namespace CCTools.Dll.AttachedLogic
{
    public static class FileSystemUtils
    {
        public static void FastCleanDir(string dir, IObserver<string> output = null)
        {
            if (Directory.Exists(dir))
            {
                var di = new DirectoryInfo(dir);
                var files = di.GetFiles("*", SearchOption.AllDirectories);
                files.AsParallel().ForAll(f =>
                {
                    try
                    {
                        f.IsReadOnly = false;
                        f.Delete();
                    }
                    catch (Exception ex)
                    {
                        output?.OnNext($"ERROR: Error cleaning file {f.Name}. Exception message: {ex.Message}");
                    }
                });
                try
                {
                    Directory.Delete(dir, true);
                }
                catch (Exception ex)
                {
                    output?.OnNext($"ERROR: Error cleaning directories. Exception message: {ex.Message}");
                }
            }
        }
    }
}