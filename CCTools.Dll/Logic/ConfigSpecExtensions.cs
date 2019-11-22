using System.Linq;
using CCTools.Dll.Model;
using FrameworkEx;
using FrameworkEx.Aspects.Invokers;

namespace CCTools.Dll.Logic
{
    internal static class ConfigSpecExtensions
    {
        private const string c_csBeginCommentToken = "# Begin of Config Spec for ";
        private const string c_buildToken = "# Build: ";
        private const string c_productIdToken = "# Product: ";
        private const string c_srBranchToken = "# SR: ";
        private const string c_mkBranchToken = " -mkbranch ";
        private const string c_targetVersionToken = "# Release: ";

        public static string GetBuild(this ConfigSpec src)
        {
            var build = src.Lines.FirstOrDefault(IsBuildLine)?.Trim().Split(' ').LastOrDefault();
            return build;
        }
        public static ProductId GetProductId(this ConfigSpec src)
        {
            var id = src.Lines.FirstOrDefault(IsCsBeginCommentLine)?.Trim().Split(' ').Get(x => x.ElementAt(x.Length - 2));
            return id.NotIn(null, string.Empty, "?") ? new ProductId(id) : src.GetProductId2();
        }
        private static ProductId GetProductId2(this ConfigSpec src)
        {
            var id = src.Lines.FirstOrDefault(IsProductIdLine)?.Trim().Split(' ').LastOrDefault();
            return id.NotIn(null, string.Empty, "?") ? new ProductId(id) : null;
        }
        public static SrBranchName GetSrBranchName(this ConfigSpec src)
        {
            var branch = src.Lines.FirstOrDefault(IsMkBranchLine)?.Trim().Split(' ').Last();
            return branch.NotIn(null, string.Empty, "?") ? new SrBranchName(branch) : src.GetSrBranchName2();
        }
        public static SrBranchName GetSrBranchName2(this ConfigSpec src)
        {
            var branch = src.Lines.FirstOrDefault(IsSrBranchLine)?.Trim().Split(' ').Last();
            return branch.NotIn(null, string.Empty, "?") ? new SrBranchName(branch) : null;
        }
        public static TargetVersion GetTargetVersion(this ConfigSpec src)
        {
            var id = src.Lines.FirstOrDefault(IsCsBeginCommentLine)?.Trim().Split(' ').LastOrDefault();
            return id.NotIn(null, string.Empty, "?") ? new TargetVersion(id) : src.GetTargetVersion2();
        }
        public static TargetVersion GetTargetVersion2(this ConfigSpec src)
        {
            var id = src.Lines.FirstOrDefault(IsTargetVersionLine)?.Trim().Split(' ').Last();
            return id.NotIn(null, string.Empty, "?") ? new TargetVersion(id) : null;
        }

        public static ViewType? GetViewType(this ConfigSpec src)
        {
            return src.GetSrBranchName() != null ? ViewType.Sr : src.GetBuild() != null ? ViewType.Build : (ViewType?)null;
        }
        public static ConfigSpec FormatCs(this ConfigSpec src)
        {
            var productIdLine = $"{c_productIdToken}{src.GetProductId()?.Name ?? "?"}";
            var targetVersionLine = $"{c_targetVersionToken}{src.GetTargetVersion()?.Value ?? "?"}";
            var srBranchName = src.GetSrBranchName();
            var build = src.GetBuild();
            var srBranchNameLine = $"{c_srBranchToken}{srBranchName?.Value ?? "?"}";
            var buildLine = $"{c_buildToken}{build ?? "?"}";
            var srBuildLines = srBranchName != null ? new[] {srBranchNameLine}
                : build != null ? new[] { buildLine }
                : new[] {srBranchNameLine, buildLine};
            var nonInformativeLines = src.Lines.Where(NotIsInformativeLine).ToArray();
            var result = new[] { productIdLine, targetVersionLine }.Concat(srBuildLines).Concat(nonInformativeLines).ToArray();
            return new ConfigSpec(result);
        }
        private static bool IsBuildLine(string src) => src.StartsWith(c_buildToken);
        private static bool IsProductIdLine(string src) => src.StartsWith(c_productIdToken);
        private static bool IsCsBeginCommentLine(string src) => src.StartsWith(c_csBeginCommentToken);
        private static bool IsSrBranchLine(string src) => src.StartsWith(c_srBranchToken);
        private static bool IsMkBranchLine(string src) => src.Contains(c_mkBranchToken);
        private static bool IsTargetVersionLine(string src) => src.StartsWith(c_targetVersionToken);
        private static bool NotIsInformativeLine(string src) => !IsBuildLine(src) && !IsProductIdLine(src) && !IsSrBranchLine(src) && !IsTargetVersionLine(src);
    }
}