using System.Collections.Generic;
using Interfaces;

namespace CCTools.Mocks
{
    public class FakeCcHelper : IClearCaseHelper
    {
        public List<string> GetConfigSpec(string viewPath)
        {
            return new List<string> {"Fake cs"};
        }

        public void SetConfigSpec(List<string> configSpec, string viewPath)
        {
            return;
        }
    }
}