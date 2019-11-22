using System.Linq;
using CCTools.Dll.Model;
using SharpCmd;

namespace CCTools.Dll.Logic
{
    internal static class ViewsLoader
    {
        public static View LoadView(string directory)
        {            
            var wdv = CommandLineExecutor.ExecuteCleartool("pwv -wdview")
                .Replace("Working directory view: ", string.Empty)
                .Trim();
            if (wdv.Contains("** NONE"))
            {
                return null;
            }
            var localRoot = CommandLineExecutor.ExecuteCleartool("pwv -root").Trim();
            var stgPath = CommandLineExecutor.ExecuteCleartool($"lsview {wdv}").Split(' ').Last().Trim();
            var root = new ViewRoot(localRoot, stgPath);
            var cs = root?.GetConfigSpec();
            var type = cs?.GetViewType();
            var view = type == null ? View.Unknown(root, cs)
                : type.Value == ViewType.Sr ? View.Sr(root, cs, cs.GetProductId(), cs.GetTargetVersion(), cs.GetSrBranchName())
                : type.Value == ViewType.Build ? View.Bld(root, cs, cs.GetProductId(), cs.GetTargetVersion(), cs.GetBuild())
                : null;
            return view;
        }        
    }
}