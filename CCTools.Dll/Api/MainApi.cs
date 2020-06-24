using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CCTools.Dll.AttachedLogic;
using CCTools.Dll.Hardcoded;
using CCTools.Dll.Logic;
using CCTools.Dll.Model;
using FrameworkEx;
using FrameworkEx.Aspects.Invokers;
using MoreLinq;
using SharpCmd;

namespace CCTools.Dll.Api
{
    public static class MainApi
    {
        public static View GetCurrentView(this string directory) => ViewsLoader.LoadView(directory);
        public static void ApplyCs(string[] cs, string localViewPath)
        {
            var sw = Stopwatch.StartNew();
            var cfgFile = $@"{localViewPath}\cs.cfg";
            Directory.SetCurrentDirectory(localViewPath);
            File.WriteAllLines(Constants.TempCsStorageFileName, cs);
            var srcElapsed = TimeSpan.Zero;
            var relElapsed = TimeSpan.Zero;
            Parallel.Invoke(() =>
            {
                var i = 0;
                CommandLineExecutor.ExecuteCleartool(new[] { $"setcs -force {Constants.TempCsStorageFileName}" },  o => o.Subscribe(s => Console.Write(++i % 2000 == 0 ? "]" : i % 1000 == 0 ? "[" : string.Empty)));
                srcElapsed = sw.Elapsed;
                Console.WriteLine($"Source files updated. Elapsed: {srcElapsed}.");
            }, 
            () =>
            {
                ProcessRelVobsRules(cs, localViewPath);
                relElapsed = sw.Elapsed;
                Console.WriteLine($"Rel vob files updated. Elapsed: {relElapsed}.");
                File.WriteAllLines(cfgFile, cs);
            });
            sw.Stop();
            Console.WriteLine($"Update done.\r\nSource: {srcElapsed}.\r\nRel: {relElapsed}.");
        }
        public static string[] GetChangelist(View view)
        {
            var fcmd = $@"cleartool find {view.ProductId.Name} -all -ele ""brtype({view.SrBranchName.Value})"" -exec ""cleartool descr -fmt \""%En\n\"" \""%CLEARCASE_PN%\""""";
            var directory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(view.ViewRoot.LocalPath);
            var files = CommandLineExecutor
                .ExecuteCmd(fcmd)
                .Split('\n')
                .SkipWhile(x => !x.Contains(">cleartool find"))
                .Skip(1)
                .TakeWhile(x => !x.Contains(">exit"))
                .Where(x => !string.IsNullOrEmpty(x.Trim()))
                .Select(x => x.Replace(view.ViewRoot.LocalPath, string.Empty).Replace("\r", string.Empty))
                .ToArray();
            Directory.SetCurrentDirectory(directory);
            return files;
        }
        public static string[] GetSrCsWeb(View view) => CssWebCsLoader.GetSrCs(view, true);
        public static string[] GetSrCsWeb(ProductId productId, TargetVersion target, string branch) => CssWebCsLoader.GetSrCs(productId, target, new SrBranchName(branch), true);
        public static string[] GetBldCsWeb(ProductId productId, TargetVersion target, string build) => CssWebCsLoader.GetBldCs(productId, target, build);

        internal static void ProcessRelVobsRules(string[] cs, string localViewPath)
        {
            var cfgFile = $@"{localViewPath}\cs.cfg";
            var curRules = GetRulesMapFromCs(File.Exists(cfgFile) ? File.ReadAllLines(cfgFile) : new string[0]);
            var newRules = GetRulesMapFromCs(cs);
            var rules = newRules.Select(rule => new
            {
                SrcRootPath = rule.Key,
                Items = rule.Value,
                Match = curRules.GetOrNull(rule.Key)?.SequenceEqual(rule.Value) ?? false
            }).ToArray();

            try
            {
                var unmatchedCount = rules.Where(x => !x.Match).Aggregate(0, (x, acc) => x+acc.Items.Length);
                if (unmatchedCount > 0)
                {
                    Console.WriteLine($"Cleaning {unmatchedCount} rel directories...");
                    var log = new StringBuilder();
                    rules.Where(x => !x.Match).ForEach(rule => rule.Items.ForEach(srcSubPath =>
                    {
                        var subPathRootDir = srcSubPath.Split('/')[0];
                        var dstSubPathRootDir = $@"{localViewPath}\{subPathRootDir}";                        
                        FileSystemUtils.FastCleanDir(dstSubPathRootDir, Observer.Create<string>(s => log.Append(s)));                        
                    }));
                    Console.WriteLine(log.ToString());
                    Console.WriteLine($"Copying {unmatchedCount} rel directories...");
                    var done = 0;
                    rules.Where(x => !x.Match).SelectMany(rule => rule.Items.SelectMany
                    (
                        srcSubPath =>
                        {
                            var isTlbx = srcSubPath == "tlbx";
                            if (isTlbx)
                            {                                
                                var subDirs = Directory.GetDirectories($@"{rule.SrcRootPath}\tlbx");
                                var subFiles = Directory.GetFiles($@"{rule.SrcRootPath}\tlbx");
                                unmatchedCount += subDirs.Length + subFiles.Length - 1;
                                var result = subDirs.Concat(subFiles).SelectArray(srcSubPathTlbx => new { rule.SrcRootPath, srcSubPath = srcSubPathTlbx.Substring(rule.SrcRootPath.Length +1).Replace('\\', '/') });
                                return result;
                            }
                            return new { rule.SrcRootPath, srcSubPath }.Unfold();
                        }
                    )).AsParallel().WithDegreeOfParallelism(100).ForAll(x =>
                    {
                        var srcRootPath = x.SrcRootPath;
                        var srcSubPath = x.srcSubPath;
                        var zipFile = Directory.Exists(srcRootPath)
                            ? Directory.GetFiles($@"{srcRootPath}\{srcSubPath.Split('/')[0]}", $"{srcSubPath.Split('/')[0]}.zip").SingleOrDefault()
                            : null;
                        var extract = zipFile != null && !Directory.GetDirectories($@"{srcRootPath}\{srcSubPath.Split('/')[0]}").Any();
                        if (extract)
                        {
                            using (var archive = ZipFile.OpenRead(zipFile))
                            {
                                archive.ExtractToDirectory(localViewPath, true);
                            }                                
                        }
                        else
                        {
                            var fromPath = $@"{srcRootPath}\{srcSubPath.Replace('/', '\\')}";
                            var toPath = $@"{localViewPath}\{srcSubPath.Replace('/', '\\')}";
                            var isFromPathDirectory = File.GetAttributes(fromPath).HasFlag(FileAttributes.Directory);
                            var from = isFromPathDirectory ? fromPath : Path.GetDirectoryName(fromPath);
                            var to = isFromPathDirectory ? $@"{toPath}\" : $@"{Path.GetDirectoryName(toPath)}\";
                            var file = isFromPathDirectory ? string.Empty : Path.GetFileName(fromPath);
                            var zipFileToken = zipFile == null ? string.Empty : $@"/xf {zipFile} ";
                            var roboCommand = $@"robocopy {from} {to} {file} /e {zipFileToken}/NFL /NDL /NJS /NJH /nc /ns /np";
                            CommandLineExecutor.ExecuteCmd(roboCommand.Unfold(), o => o.Subscribe(s => { }));
                        }
                        var remaining = unmatchedCount - Interlocked.Increment(ref done);
                        Console.WriteLine(extract ? $"Extracted {Path.GetFileName(zipFile)} ({remaining})." : $"Copied {srcSubPath} ({remaining}).");
                    });
                }                
            }
            finally
            {
                if (File.Exists(cfgFile)) File.Delete(cfgFile);
                File.WriteAllLines(cfgFile, cs);
            }            
        }
        private static Dictionary<string, string[]> GetRulesMapFromCs(IEnumerable<string> rulesFileContent)
        {
            return rulesFileContent
                .Where(x => x.StartsWith("#stgsrvload"))
                .Select(x => x.Split(' ').Get(t => new {SrcRootPath = t[2], SrcSubPath = t[1].Substring(1)}))
                .GroupBy(x => x.SrcRootPath)
                .ToDictionary(x => x.Key, x => x.Select(i => i.SrcSubPath).OrderBy(i => i).ToArray());
        }
    }
}