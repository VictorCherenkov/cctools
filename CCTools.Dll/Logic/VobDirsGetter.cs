using System.IO;
using CCTools.Dll.Model;

namespace CCTools.Dll.Logic
{
    internal static class VobDirsGetter
    {
        public static string GetVobDir(this View src)
        {
            return src.ProductId.Name == "CGAFE"? Path.Combine(src.ViewRoot.LocalPath, @"CGAFE") : src.ViewRoot.LocalPath;
        }
        public static string GetProductsDir(this View src)
        {
            return src.ProductId.Name == "CGAFE" ? Path.Combine(src.ViewRoot.LocalPath, @"CGAFE\Products") : null;
        }
    }
}