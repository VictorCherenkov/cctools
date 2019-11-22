using System.IO;
using System.Linq;
using System.Reflection;
using CCTools.Dll.Model;
using FrameworkEx.Aspects;
using FrameworkEx.Aspects.Wrappers;
using log4net;

namespace CCTools.Dll.Logic
{
    internal static class ViewRootsScanner
    {
        private static readonly ILog s_logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static ViewRoot[] GetViewsRoots(LsViewsInfo context, string directory)
        {
            var isViewRoot = IsViewRoot(directory);
            var subDirs = directory
                .ToFunc(x => isViewRoot ? new string[0] : Directory.GetDirectories(x, "*.*", SearchOption.TopDirectoryOnly))
                .WrapWithTryCatch(ex => s_logger.Error(string.Empty, ex), new string[0])
                .Invoke();
            if (isViewRoot)
            {
                var uuid = File.ReadAllText(Path.Combine(directory, "view.dat")).Split(':')[2];
                var serverAccessPath = context.Views.First(x => x.Uuid.Replace(":", string.Empty).Replace(".", string.Empty) == uuid).ServerAccessPath;
                return new[] { new ViewRoot(directory, serverAccessPath) }.Concat(subDirs.SelectMany(x => GetViewsRoots(context, x)).ToArray()).ToArray();
            }
            return subDirs.SelectMany(x => GetViewsRoots(context, x)).ToArray();
        }
        private static bool IsViewRoot(string directory)
        {
            return new DirectoryInfo(directory)
                .ToFunc(x => x.GetFiles("view.dat", SearchOption.TopDirectoryOnly).Any())
                .WrapWithTryCatch(ex => s_logger.Error(string.Empty, ex))
                .Invoke();
        }        
    }
}