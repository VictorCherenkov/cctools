using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using CCTools.Dll.Logic;
using CCTools.Dll.Model;

namespace CCTools.Dll.Hardcoded
{
    internal static class Constants
    {
        private static readonly string s_programDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string s_scriptsDirectory = Path.Combine(s_programDirectory, "Scripts");

        public static readonly string GetSrCsScriptFileName = "Get-SrConfigSpec.ps1";
        public static readonly string GetBldCsScriptFileName = "Get-BldConfigSpec.ps1";
        public static readonly string GetSrCsScriptFullFileName = Path.Combine(s_scriptsDirectory, GetSrCsScriptFileName);
        public static readonly string GetBldCsScriptFullFileName = Path.Combine(s_scriptsDirectory, GetBldCsScriptFileName);
        public static readonly string TempCsStorageFileName = Path.Combine(s_programDirectory, "cs.txt");
        public static readonly string HostName = Environment.MachineName;
        public static readonly ViewsBaseDirectory BaseDirIfNoViews = new ViewsBaseDirectory(@"C:\dev\amat\cc", @"\\HS7PW12\cc");
    }
}