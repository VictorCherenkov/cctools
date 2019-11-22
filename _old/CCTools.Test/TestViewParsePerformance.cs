using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace CCTools.Test
{
    [TestFixture]
    public class TestViewParsePerformance
    {
        [Test]
        public void Test()
        {
            string content = File.ReadAllText("Sample.txt");
            string hostName = @"\\" + Environment.MachineName;
            string hostNameLower = hostName.ToLower();
            Stopwatch sw = Stopwatch.StartNew();
            var list = content
                .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(v => v.ToLower().Contains(hostNameLower))
                .Select(v => v.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1])                
                .Where(v => !v.Trim().EndsWith("Views") && Directory.Exists(v))
                .Select(v => Path.GetDirectoryName(v).Replace(@"\\" + hostName, string.Empty).Replace(@"\\" + hostNameLower, string.Empty))
                .Distinct()
                .ToList();
            sw.Stop();
            Console.WriteLine("Time Elapsed: {0} ms", sw.ElapsedMilliseconds);
            Console.WriteLine(string.Join("\r\n", list));
        }
    }
}