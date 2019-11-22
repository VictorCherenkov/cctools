using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Interfaces;

namespace CCTools.Model
{
    public class MainModel
    {
        private readonly IClearCaseHelper m_clearCaseHelper;
        private readonly ICmdRunnerFactory m_cmdRunnerFactory;
        private readonly ICssWebHelper m_cssWebHelper;

        private Lazy<IList<string>> m_localViews;

        private ILogger m_logger;

        public MainModel(IClearCaseHelper clearCaseHelper, ICmdRunnerFactory cmdRunnerFactory, ICssWebHelper cssWebHelper)
        {
            m_cmdRunnerFactory = cmdRunnerFactory;
            m_cssWebHelper = cssWebHelper;
            m_clearCaseHelper = clearCaseHelper;
            m_localViews = new Lazy<IList<string>>(GetLocalViews);
        }

        public void SetLogger(ILogger logger)
        {
            m_logger = logger;
        }

        public IList<string> LocalViews
        {
            get { return m_localViews.Value; }
        }

        public void RefreshViewsList()
        {
            m_localViews = new Lazy<IList<string>>(GetLocalViews);
        }

        public ICmdRunner UpdateViewRunner(string viewPath)
        {
            return m_cmdRunnerFactory.CreateUpdateRunner(viewPath, m_logger);
        }

        public string GetViewName(string viewPath)
        {
            return viewPath.Split('\\').Last();
        }

        public bool IsSrView(string viewPath)
        {
            return viewPath.Split('\\').Any(s => s.Equals("sr", StringComparison.InvariantCultureIgnoreCase));
        }

        public bool IsRelView(string viewPath)
        {
            return !IsSrView(viewPath);
        }

        public void BrowseTotal(string viewPath)
        {
            Process.Start(@"C:\totalcmd\totalcmd.exe ", GetMakeDir("C:" + viewPath));
        }

        public void Browse(string viewPath)
        {
            Process.Start("explorer.exe", GetMakeDir(viewPath));
        }

        public void RemoveCcUpdateLogs(string viewPath)
        {
            foreach (string fileName in Directory.GetFiles(viewPath, "*.updt"))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public string GetMakeDir(string viewPath)
        {
            if (IsCgafeView(viewPath))
            {
                return viewPath + @"\CGAFE\Products";
            }
            if (IsCgafeBaselineView(viewPath))
            {
                return viewPath + @"\CGAFEBaseline\Products\CGAFEBaseline";
            }
            if (IsSilverView(viewPath))
            {
                return viewPath + @"\Silver\Products\Silver";
            }
            if (IsWisrView(viewPath))
            {
                return viewPath + @"\iBox\Products";
            }
            if (IsRaiderView(viewPath))
            {
                return viewPath + @"\Raider\Products\Raider";
            }
            return string.Empty;
            //throw new Exception(string.Format("Can't resolve this view. View path : {0}", viewPath));
        }

        public string GetNewConfigSpecForSrView(string viewPath, string release)
        {
            string view = GetViewName(viewPath);
            return m_cssWebHelper.GetNewConfigSpecFromWeb(
                GetProductNameForSrView(view), 
                !string.IsNullOrEmpty(release) ? release : "LATEST", 
                view);
        }

        public string GetNewConfigSpecForRelView(string viewPath)
        {
            string view = GetViewName(viewPath);
            return m_cssWebHelper.GetNewConfigSpecFromWeb(
                GetProductNameForRelView(view), 
                view.Contains("_") ? view.Substring(view.IndexOf("_") + 1) : "LATEST", 
                "srXXXX");
        }

        public bool IsTestPostBuildEnabled(string viewPath)
        {
            string fileName = string.Format(@"{0}\{1}", GetMakeDir(viewPath), "TestPostBuild.cmd");
            if (!File.Exists(fileName))
            {
                return true;
            }

            string[] content = File.ReadAllLines(fileName);
            if (!content[0].Contains("goto end"))
            {
                throw new Exception("Invalid test post build file content.");
            }

            return content[0].Contains("rem");            
        }

        public void SwitchTestPostBuildStatus(string viewPath)
        {
            string fileName = string.Format(@"{0}\{1}", GetMakeDir(viewPath), "TestPostBuild.cmd");
            if (!File.Exists(fileName))
            {
                return;
            }
            if (IsTestPostBuildEnabled(viewPath))
            {
                DisableTestPostBuild(fileName);
            }
            else
            {
                EnableTestPostBuild(fileName);
            }
        }

        public ICmdRunner CreateMakeCleanAndDebugRunner(string viewPath)
        {
            return m_cmdRunnerFactory.CreateCleanAndDebugMakeRunner(GetMakeDir(viewPath), m_logger);
        }

        public ICmdRunner CreateMakeDebugRunner(string viewPath)
        {
            return m_cmdRunnerFactory.CreateDebugMakeRunner(GetMakeDir(viewPath), m_logger);
        }

        public ICmdRunner CreateMakeCleanAndReleaseRunner(string viewPath)
        {
            return m_cmdRunnerFactory.CreateCleanAndReleaseMakeRunner(GetMakeDir(viewPath), m_logger);
        }

        public ICmdRunner CreateMakeReleaseRunner(string viewPath)
        {
            return m_cmdRunnerFactory.CreateReleaseMakeRunner(GetMakeDir(viewPath), m_logger);
        }

        public string[] GetCs(string viewPath)
        {
            return m_clearCaseHelper.GetConfigSpec(viewPath).ToArray();
        }
        public void SetCs(string viewPath, string[] cs)
        {
            m_clearCaseHelper.SetConfigSpec(new List<string>(cs), viewPath);
        }

        public ICmdRunner[] RestartEchoRunners()
        {
            return new[]
                       {
                           m_cmdRunnerFactory.CreateStopEchoRunner(m_logger), 
                           m_cmdRunnerFactory.CreateStartEchoRunner(m_logger)
                       };
        }

        public void ViewUpdateLog(string viewPath)
        {
            Process p = Process.Start(GetLastUpdateLogFileName(viewPath));
            if (p != null)
            {
                p.WaitForExit();
            }
        }

        public SyncVobData GetSyncVobData(string viewPath)
        {
            string viewName = GetViewName(viewPath);
            string vobName = IsSrView(viewPath) ? GetProductNameForSrView(viewName) : GetProductNameForRelView(viewName);
            return m_cssWebHelper.GetSyncVobData(vobName);
        }

        public SyncVobData GetSyncVobDataForVob(string vobName)
        {
            return m_cssWebHelper.GetSyncVobData(vobName);
        }

        public void DeleteViewFilesLight(string viewPath)
        {
            DirectoryInfo di = new DirectoryInfo(GetCurViewMainVobDir(viewPath));
            foreach (FileInfo file in di.GetFiles().Where(f => !f.Name.EndsWith("view.dat")))
            {
                file.Attributes = FileAttributes.Normal;
                file.Delete();
            }

            foreach (DirectoryInfo directory in di.GetDirectories().Where(d => !d.Name.EndsWith("view.stg")))
            {
                DeleteFolder(directory);
            }
        }

        private static void DeleteFolder(DirectoryInfo di)
        {
            foreach (DirectoryInfo dif in di.GetDirectories())
            {
                DeleteFolder(dif);
            }
     
            foreach (FileInfo fi in di.GetFiles())
            {
                try
                {
                    fi.Attributes = FileAttributes.Normal;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            try
            {
                di.Delete(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private IList<string> GetLocalViews()
        {            
            var log = new StringLogger();
            string hostName = @"\\" + Environment.MachineName;
            string hostNameLower = hostName.ToLower();
            m_cmdRunnerFactory.CreateRunnerGetHostViews(hostName, log).Run();
            return log.OutputString
                .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(v => v.ToLower().Contains(hostNameLower))
                .Select(v => v.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1])
                .Where(v => !v.Trim().EndsWith("Views") && Directory.Exists(v))
                .Select(v => Path.GetDirectoryName(v).Replace(hostName, string.Empty).Replace(hostNameLower, string.Empty))
                .Distinct()
                .ToList();
        }

        private static string GetLastUpdateLogFileName(string viewPath)
        {
            return Directory.GetFiles(viewPath, "*.updt")
                .OrderByDescending(File.GetLastWriteTime)
                .FirstOrDefault();
        }

        private static void DisableTestPostBuild(string fileName)
        {
            RemoveReadOnlyLock(fileName);
            string[] lines = File.ReadAllLines(fileName);
            lines[0] = "goto end";
            File.WriteAllLines(fileName, lines);
        }

        private static void EnableTestPostBuild(string fileName)
        {
            RemoveReadOnlyLock(fileName);
            string[] lines = File.ReadAllLines(fileName);
            lines[0] = "rem goto end";
            File.WriteAllLines(fileName, lines);
        }

        private static void RemoveReadOnlyLock(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            if ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                fi.Attributes = fi.Attributes & ~FileAttributes.ReadOnly;
            }
        }

        private static string GetCurViewMainVobDir(string viewPath)
        {
            string vobDir = string.Empty;
            if (IsCgafeView(viewPath))
            {
                vobDir = viewPath + @"\CGAFE";
            }
            else if (IsCgafeBaselineView(viewPath))
            {
                vobDir = viewPath + @"\CGAFEBaseline";
            }
            else if (IsSilverView(viewPath))
            {
                vobDir = viewPath + @"\Silver";
            }
            else if (IsWisrView(viewPath))
            {
                vobDir = viewPath + @"\iBox";
            }
            return vobDir;
        }

        private static bool IsCgafeView(string viewPath)
        {
            return Directory.GetDirectories(viewPath, "CGAFE").Length > 0;
        }

        private static bool IsCgafeBaselineView(string viewPath)
        {
            return Directory.GetDirectories(viewPath, "CGAFEBaseline").Length > 0;
        }

        private static bool IsSilverView(string viewPath)
        {
            return Directory.GetDirectories(viewPath, "Silver").Length > 0;
        }

        private static bool IsWisrView(string viewPath)
        {
            return Directory.GetDirectories(viewPath, "iBox").Length > 0;
        }

        private static bool IsRaiderView(string viewPath)
        {
            return Directory.GetDirectories(viewPath, "RAIDER").Length > 0;
        }

        private static string GetProductNameForSrView(string branchName)
        {
            string productName = branchName.Substring(0, branchName.IndexOf("sr"));

            switch (productName.ToUpper())
            {
                case "CGAFE":
                    productName = "CGAFE";
                    break;
                case "SL":
                    productName = "Silver";
                    break;
            }
            return productName;
        }

        private static string GetProductNameForRelView(string branchName)
        {
            string productName = branchName.Contains("_") ? branchName.Substring(0, branchName.IndexOf("_")) : branchName;
            
            switch (productName.ToUpper())
            {
                case "CGAFE":
                    productName = "CGAFE";
                    break;
                case "CGAFEBASELINE":
                    productName = "CGAFEBa";
                    break;
                case "SILVER":
                    productName = "Silver";
                    break;
            }
            return productName;
        }

        public ICmdRunner CreateSrView(string branchName)
        {
            return m_cmdRunnerFactory.CreateRunnerCreateView(branchName, m_logger);
        }

        public ICmdRunner CreateBranchType(string branchName)
        {
            return m_cmdRunnerFactory.CreateRunnerCreateBranch(branchName, GetProductNameForSrView(branchName), m_logger);
        }
    }
}