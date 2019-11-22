using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.XPath;
using HtmlAgilityPack;
using NUnit.Framework;

namespace CCTools.Test
{
    [TestFixture]
    public class TestCssWebHelper
    {

        [Test]
        public void TestSyncVobContentParse()
        {
            string content = File.ReadAllText("vobepoch.plx.html");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            IEnumerable<string> headersAbbr = GetHeadersAbb(doc);
            IEnumerable<string> headers = GetHeaders(doc);
            int totalWidth = headers.Max(s => s.Length);
            int rowsCount = headers.Count();
            IEnumerable<IEnumerable<string>> allValues = GetAllValues(doc, rowsCount)
                .Select((v, i) => new { Values = v, Index = i })
                .Join(headers.Select((h, i) => new {Header = h, Index = i}),
                      v => v.Index,
                      h => h.Index,
                      (v, h) => new[] { h.Header.PadRight(totalWidth) }.Concat(v.Values));
            IEnumerable<string> vobs = GetVobs(doc);
            Console.WriteLine(string.Empty.PadRight(totalWidth) + string.Join(" ", headersAbbr));
            foreach (IEnumerable<string> rowValues in allValues)
            {
                Console.WriteLine(string.Join(" ", rowValues));
            }

            Console.WriteLine(string.Join("\r\n", vobs.Select(s => string.Format("\"{0}\",", s))));
        }

        private IEnumerable<string> GetHeadersAbb(HtmlDocument doc)
        {
            XPathNodeIterator topHeaders = doc.CreateNavigator().Select("/html[1]/body[1]/table[2]/tbody[1]/tr[1]/th");
            return topHeaders.Cast<XPathNavigator>().Skip(1).Select(h => h.InnerXml);
        }
        private IEnumerable<string> GetHeaders(HtmlDocument doc)
        {
            XPathNodeIterator trows = doc.CreateNavigator().Select("//tr/th[@*]");
            return trows.Cast<XPathNavigator>().Skip(1).Select(h => h.InnerXml);
        }
        private IEnumerable<IEnumerable<string>> GetAllValues(HtmlDocument doc, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                yield return GetRowValues(doc, i + 1);
            }
        }
        private IEnumerable<string> GetRowValues(HtmlDocument doc, int rowNumber)
        {
            XPathNodeIterator trows = doc.CreateNavigator().Select(string.Format("//tbody/tr[{0}]/td", rowNumber + 1));
            return trows.Cast<XPathNavigator>().Select(
                h => h.InnerXml.StartsWith("<b>", StringComparison.InvariantCultureIgnoreCase) ?
                    h.SelectSingleNode(string.Format("//tbody/tr[{0}]/td/b/font", rowNumber + 1)).InnerXml :
                    h.InnerXml);
        }

        private IEnumerable<string> GetVobs(HtmlDocument doc)
        {
            XPathNodeIterator trows = doc.CreateNavigator().Select("//option[@value]");
            return trows.Cast<XPathNavigator>().Select(h => h.GetAttribute("value", string.Empty));
        }
    }
}
