using System;
using System.IO;
using System.Linq;
using CCTools.Dll.Hardcoded;
using CCTools.Dll.Model;
using FrameworkEx.Aspects.Invokers;
using MoreLinq;
using SharpCmd;

namespace CCTools.Dll.Logic
{
    public static class LsViewInfoLoader
    {
        public static LsViewsInfo LoadLsViewInfos()
        {
            var lines = CommandLineExecutor.ExecuteCleartool($"lsview -host {Environment.MachineName} -quick -long")
                .Split(new[] { "\r\n" }, StringSplitOptions.None)
                .Where(x => x.StartsWith("View server access path:") || x.StartsWith("View uuid:"))
                .ToArray();
            var viewBaseDir = lines.Any() ? lines.First(x => x.StartsWith("View server access path:")).Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1].Get(x =>
            {
                var slashCount = x.Count(c => c == '\\');
                var slashNumberToGet = slashCount - 3;
                var slashIndex = x.Aggregate(new {Index = 0, Slashes = 0, Result = -1}, (r, c) => new
                {
                    Index = r.Index + 1,
                    Slashes = c == '\\' ? r.Slashes + 1 : r.Slashes,
                    Result = r.Slashes == slashNumberToGet - 1 && c == '\\' ? r.Index : r.Result
                }, r => r.Result);
                var localPath = x.Substring(0, slashIndex);
                var networkPath = $@"\\{Constants.HostName}\{Path.GetFileName(localPath)}";
                return new ViewsBaseDirectory(localPath, networkPath);
            }) : Constants.BaseDirIfNoViews;

            var views = lines.Batch(2).Select(x => new LsViewInfo
            (            
                x.First().Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1],
                x.Skip(1).First().Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1]
            )).ToArray();
            return new LsViewsInfo(viewBaseDir, views);
        }
    }
}