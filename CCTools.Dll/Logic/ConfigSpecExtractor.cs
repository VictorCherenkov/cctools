using System.IO;
using CCTools.Dll.Model;

namespace CCTools.Dll.Logic
{
    internal static class ConfigSpecExtractor
    {
        public static ConfigSpec GetConfigSpec(this ViewRoot src)
        {
            var rawConfigSpec = new ConfigSpec(File.ReadAllLines(Path.Combine(src.ServerAccessPath, "config_spec")));
            return rawConfigSpec.FormatCs();
        }
    }
}