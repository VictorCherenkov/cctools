using System;
using System.Linq;
using CCTools.Dll.Hardcoded;
using CCTools.Dll.Model;
using FrameworkEx;
using MoreLinq;
using SharpCmd;

namespace CCTools.Dll.Logic
{
    internal static class CssWebCsLoader
    {
        public static string[] GetSrCs(View view, bool optimize)
        {
            return GetSrCs(view.ProductId, view.TargetVersion, view.SrBranchName, optimize);
        }
        public static string[] GetSrCs(ProductId product, TargetVersion target, SrBranchName sr, bool optimize)
        {
            var output = CommandLineExecutor.ExecuteCmd(GetSrCsCommandText(product, target, sr, optimize));
            return output.Split(new[] { "\r\n" }, StringSplitOptions.None)
                .SkipUntil(x => x.Contains(Constants.GetSrCsScriptFileName))
                .TakeWhile(x => !x.EndsWith(">exit"))
                .Select(x => x.Replace("# stgsrvload", "#stgsrvload"))
                .ToArray();
        }
        public static string[] GetBldCs(ProductId product, TargetVersion target, string build)
        {
            var output = CommandLineExecutor.ExecuteCmd(GetBldCsCommandText(product, target, build));
            return output.Split(new[] { "\r\n" }, StringSplitOptions.None)
                .SkipUntil(x => x.Contains(Constants.GetBldCsScriptFileName))
                .TakeWhile(x => !x.EndsWith(">exit"))
                .Select(x => x.Replace("# stgsrvload", "#stgsrvload"))
                .ToArray();
        }
        private static string GetSrCsCommandText(ProductId product, TargetVersion target, SrBranchName sr, bool optimize)
        {
            return $"powershell {Constants.GetSrCsScriptFullFileName} {product.Name} {target.Value} {sr.Value} false true {optimize.AsExactString()} true";
        }
        private static string GetBldCsCommandText(ProductId product, TargetVersion target, string build)
        {
            return $"powershell {Constants.GetBldCsScriptFullFileName} {product.Name} {target.Value} {build}";
        }
    }
}