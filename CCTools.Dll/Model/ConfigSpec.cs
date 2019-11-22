using System.Collections.Generic;
using System.Linq;

namespace CCTools.Dll.Model
{
    public class ConfigSpec
    {
        public string[] Lines { get; private set; }
        public ConfigSpec(IEnumerable<string> lines)
        {
            Lines = lines.ToArray();
        }
    }
}