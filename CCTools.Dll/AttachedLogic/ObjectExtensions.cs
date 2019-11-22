using System.Linq;

namespace CCTools.Dll.AttachedLogic
{
    public static class ObjectExtensions
    {
        public static bool IsOneOf<T>(this T src, params T[] set)
        {
            return set.Contains(src);
        }
    }
}