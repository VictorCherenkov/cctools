using System;
using System.Collections.Generic;
using Interfaces;

namespace ClearCase
{
    public class ClearcaseHelper : IClearCaseHelper
    {
        private static readonly Application s_ccApp = new Application();

        public List<string> GetConfigSpec(string viewPath)
        {
            var cfgSpec = s_ccApp.View[viewPath].ConfigSpec;
            return new List<string>(cfgSpec.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
        }

        public void SetConfigSpec(List<string> configSpec, string viewPath)
        {
            s_ccApp.View[viewPath].ConfigSpec = LinesAsString(configSpec);
        }        

        private static string LinesAsString(List<string> strings)
        {
            return string.Join("\n", strings.ToArray());                
        }
    }
}
