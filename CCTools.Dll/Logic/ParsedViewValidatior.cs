using System.Linq;
using CCTools.Dll.Model;
using FrameworkEx;

namespace CCTools.Dll.Logic
{
    internal static class ParsedViewValidatior
    {
        public static bool IsValidView(this View src)
        {
            return src?.ViewRoot != null 
                && src.ViewRoot.LocalPath.IsNotNullOrEmpty()
                && src.ViewRoot.ServerAccessPath.IsNotNullOrEmpty()
                && src.ConfigSpec != null 
                && src.ConfigSpec.Lines.Any();
        }
    }
}