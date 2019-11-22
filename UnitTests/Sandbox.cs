using System.IO;
using CCTools.Dll.Api;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class Sandbox
    {
        [Test]
        public void Test()
        {
            var cs = File.ReadAllLines(@"c:\temp\test\cs.txt");
            const string c_viewPath = @"C:\dev\amat\cc\dream_2.75_1";
            MainApi.ProcessRelVobsRules(cs, c_viewPath);
        }
    }
}
