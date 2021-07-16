using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CCTools.Dll.Api;
using CCTools.Dll.AttachedLogic;
using CCTools.Dll.Model;
using FrameworkEx;
using MoreLinq;
using SharpCmd;

namespace CCTools.Cmd
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var folder = Environment.CurrentDirectory;
            var view = folder.GetCurrentView();
            var cmd = args.FirstOrDefault();
            if (cmd == Commands.ConfigSpec)
            {
                var productId = new ProductId(args[1]);
                var targetVersion = new TargetVersion(args[2]);
                var branch = "jsrXXXXXX_VC";
                var cs = MainApi.GetSrCsWeb(productId, targetVersion, branch);
                cs.ForEach(line => Console.WriteLine(line));
            }
            else if (cmd == Commands.LoadRelVobs)
            {
                var productId = new ProductId(args[1]);
                var targetVersion = new TargetVersion(args[2]);
                var branch = "jsrXXXXXX_VC";
                var localViewPath = Directory.GetParent(folder).FullName;
                if (!Directory.Exists(Path.Combine(folder, "Products")))
                {
                    Console.WriteLine("Directory does not appear to be a view main VOB directory. Please type YES if you force to continue.");
                    if (Console.ReadLine().ToUpper() != "YES")
                    {
                        return;
                    }
                }
                var cs = MainApi.GetSrCsWeb(productId, targetVersion, branch);
                cs.ForEach(line => Console.WriteLine(line));
                MainApi.ApplyCs(cs, localViewPath);
            }

            else if (cmd != Commands.Cd)
                Console.WriteLine(view?.DisplayText ?? $"{folder} is not ClearCase view path");
            var canOperateOnNonCcViews = args.Length > 3 && cmd.IsOneOf(Commands.NewSr, Commands.NewBuild);
            if (!canOperateOnNonCcViews && (!args.Any() || view?.ViewType == null || view.ViewType.Value != ViewType.Sr))
                return;
            if (cmd == Commands.Help)
            {
                var commands = typeof(Commands)
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(x => x.IsLiteral && !x.IsInitOnly)
                    .Select(x => (string) x.GetRawConstantValue())
                    .ToArray();
                Console.WriteLine("These commands are defined:");
                Console.WriteLine(commands.ToLinesString());
            }
            else if (cmd == Commands.FullUpdate)
            {
                Console.WriteLine("Getting latest config spec from css...");
                var cs = MainApi.GetSrCsWeb(view);
                Console.WriteLine("Applying config spec...");
                MainApi.ApplyCs(cs, view.ViewRoot.LocalPath);
            }
            else if (cmd == Commands.FullUpdateGit)
            {
                var gitDir = Directory.GetCurrentDirectory();
                Console.WriteLine("Getting latest config spec from css...");
                var cs = MainApi.GetSrCsWeb(view);
                Console.WriteLine("Applying config spec...");
                MainApi.ApplyCs(cs, view.ViewRoot.LocalPath);
                Console.WriteLine("Syncing with git...");
                Directory.SetCurrentDirectory(gitDir);
                var i = 0;
                var log = CommandLineExecutor.ExecuteCmd(@"python ""c:\bin2\git-cc-master\gitcc"" update sync", o => o.Subscribe(s => Console.Write(++i % 2000 == 0 ? ">" : i % 1000 == 0 ? "<" : string.Empty)));
                Console.WriteLine(log);
            }
            else if (cmd == Commands.ApplyCs)
            {
                var cs = view.ConfigSpec.Lines;
                Console.WriteLine("Applying config spec...");
                MainApi.ApplyCs(cs, view.ViewRoot.LocalPath);
            }
            else if (cmd == Commands.ShowChangelist)
            {
                MainApi.GetChangelist(view).ForEach(x => Console.WriteLine(x));
            }
            else if (cmd == Commands.NewSr)
            {
                var branch = args[1];
                var productId = args.Length > 3 ? new ProductId(args[2]) : view.ProductId;
                var targetVersion = args.Length > 3 ? new TargetVersion(args[3]) : view.TargetVersion;
                if (view == null)
                {
                    var viewDirName = args.Length > 4 ? args[4] : string.Empty;
                    Console.WriteLine("Preparing new view environment...");
                    var currentDir = Directory.GetCurrentDirectory();
                    var tag = $"{Environment.MachineName}_{viewDirName}";
                    var baseDir = new ViewsBaseDirectory($"{currentDir}", $"\\\\{Environment.MachineName}\\{Path.GetFileName(currentDir)}");
                    var networkViewPath = Path.Combine(baseDir.NetworkPath, viewDirName);
                    var localViewPath = Path.Combine(baseDir.LocalPath, viewDirName);
                    Console.WriteLine($"currentDir = {currentDir}");
                    Console.WriteLine($"viewDirName = {viewDirName}");
                    Console.WriteLine($"tag = {tag}");
                    Console.WriteLine($"baseDir.NetworkPath = {baseDir.NetworkPath}");
                    Console.WriteLine($"baseDir.LocalPath = {baseDir.LocalPath}");
                    Console.WriteLine($"networkViewPath = {networkViewPath}");
                    Console.WriteLine($"localViewPath = {localViewPath}");
                    Console.WriteLine("Creating new view...");
                    CommandLineExecutor.ExecuteCleartool($"mkview -snapshot -tag {tag} \"{networkViewPath}\"");
                    Directory.SetCurrentDirectory(localViewPath);
                    view = folder.GetCurrentView();
                }
                Console.WriteLine("Loading config spec...");
                var cs = MainApi.GetSrCsWeb(productId, targetVersion, branch);
                var makeBranch = !branch.ToLower().Contains("xxx");
                if (makeBranch)
                {
                    var productToBranch = cs.FirstOrDefault(line => line.StartsWith("# Product: "))?.Trim().Split(' ').LastOrDefault() ?? productId.Name;
                    Console.WriteLine($"Making branch {branch} for product {productToBranch}...");
                    CommandLineExecutor.ExecuteCleartool($@"mkbrtype -c . {branch}@\{productToBranch}");
                }
                Console.WriteLine("Applying config spec...");
                MainApi.ApplyCs(cs, view.ViewRoot.LocalPath);
            }
            else if (cmd == Commands.NewBuild)
            {
                var productId = new ProductId(args[1]);
                var targetVersion = new TargetVersion(args[2]);
                var build = args[3];
                if (view == null)
                {
                    var viewDirName = build;
                    Console.WriteLine("Preparing new view environment...");
                    var currentDir = Directory.GetCurrentDirectory();
                    var tag = $"{Environment.MachineName}_{viewDirName}";
                    var baseDir = new ViewsBaseDirectory($"{currentDir}", $"\\\\{Environment.MachineName}\\{Path.GetFileName(currentDir)}");
                    var networkViewPath = Path.Combine(baseDir.NetworkPath, viewDirName);
                    var localViewPath = Path.Combine(baseDir.LocalPath, viewDirName);
                    Console.WriteLine($"currentDir = {currentDir}");
                    Console.WriteLine($"viewDirName = {viewDirName}");
                    Console.WriteLine($"tag = {tag}");
                    Console.WriteLine($"baseDir.NetworkPath = {baseDir.NetworkPath}");
                    Console.WriteLine($"baseDir.LocalPath = {baseDir.LocalPath}");
                    Console.WriteLine($"networkViewPath = {networkViewPath}");
                    Console.WriteLine($"localViewPath = {localViewPath}");
                    Console.WriteLine("Creating new view...");
                    CommandLineExecutor.ExecuteCleartool($"mkview -snapshot -tag {tag} \"{networkViewPath}\"");
                    Directory.SetCurrentDirectory(localViewPath);
                    view = folder.GetCurrentView();
                }
                Console.WriteLine("Loading config spec...");
                var cs = MainApi.GetBldCsWeb(productId, targetVersion, build);
                Console.WriteLine("Applying config spec...");
                MainApi.ApplyCs(cs, view.ViewRoot.LocalPath);
            }
            else if (cmd == Commands.MergeToRemote)
            {
                Console.WriteLine("Getting changelist...");
                var files = MainApi.GetChangelist(view);
                var filesCut = files.Where(x => !files.Any(dir => x != dir && x.Contains(dir))).ToArray();
                filesCut.ForEach(x => Console.WriteLine(x));
                var to = args.Length > 1 ? args[1] : "mrg_latest";
                Process.Start("clearmrgman", $@"/t {to} /b {view.SrBranchName.Value} /n {string.Join(" ", filesCut)}");
            }
            else if (cmd == Commands.MergeToRemoteSlow)
            {
                var to = args.Length > 1 ? args[1] : "mrg_latest";
                Process.Start("clearmrgman", $@"/t {to} /b {view.SrBranchName.Value}");
            }
            else if (cmd == Commands.MergeFromRemote)
            {
                var from = args.Length > 1 ? args[1] : "main";
                Process.Start("clearmrgman", $@"/t {view.ViewRoot.LocalPath} /f {from} /n \{view.ProductId.Name}");
            }
            else if (cmd == Commands.OpenBrtool)
            {
                Process.Start("brtool", $@"{view.SrBranchName.Value}");
            }
            else if (cmd == Commands.ShowVersionTree)
            {
                var fileName = args[1];
                var fullFileName = Path.Combine(folder.Replace(".git", string.Empty), fileName);
                Process.Start("clearvtree", $@"{fullFileName}");
            }
            else if (cmd == Commands.OpenJira)
            {
                if (view.SrBranchName.Value.StartsWith("jsr"))
                {
                    Process.Start("chrome.exe", $@"http://sc-css-devjira/jira/browse/SR-{view.SrBranchName.Value.Substring(3)}");
                }
            }
            else if (cmd == Commands.ShowHistory)
            {
                var fileName = args[1];
                var fullFileName = Path.Combine(folder.Replace(".git", string.Empty), fileName);
                CommandLineExecutor.ExecuteCleartool($"lshistory -graphical {fullFileName}");
            }
            else if (cmd == Commands.Cd)
            {
                var fileName = args[1];
                var fullFileName = Path.Combine(folder.Replace(".git", string.Empty), fileName);
                var dir = Path.GetDirectoryName(fullFileName);
                Clipboard.SetText($"cd {dir}");
            }
            else if (cmd == Commands.Annotate)
            {
                var fileName = args[1];
                var fullFileName = Path.Combine(folder.Replace(".git", string.Empty), fileName);
                CommandLineExecutor.ExecuteCleartool($"annotate {fullFileName}");
                Process.Start($"{fullFileName}.ann");
            }
            else if (cmd == Commands.CheckinToClearCase)
            {
                CommandLineExecutor.ExecuteCmd(new[]
                {
                    @"git add -A",
                    @"git commit -m ""none""",
                    @"gitcc checkin"
                }, o => o.Subscribe(Console.Write));                
            }
            else
            {
                Console.WriteLine("Unknown command: {0}", cmd);
            }
        }        
    }
}
