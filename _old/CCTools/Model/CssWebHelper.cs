using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace CCTools.Model
{
    public class CssWebHelper : ICssWebHelper
    {
        public string GetNewConfigSpecFromWeb(string productName, string release, string srNum)
        {
            string cs = GetContent(string.Format(@"http://css/www1/cmweb/services/cmweb.srvc.config-spec.php?ServiceCMD=QuickGet&productId={0}&release={1}&sr={2}&makeSR=false&useNB=false&ignoreBTV=true&outputFormat=text", productName, release, srNum));
            Regex regex = new Regex("\n(mkbranch .*)\r\n");
            IEnumerable<Match> matchs = regex.Matches(cs).Cast<Match>();
            foreach (Match match in matchs)
            {
                string mkBranchLine = match.Groups[1].Value;
                if (!mkBranchLine.Contains(srNum))
                {
                    cs = cs.Replace(mkBranchLine, "#" + mkBranchLine);
                }
            }
            return cs;
        }

        public SyncVobData GetSyncVobData(string vobName)
        {
            string content = GetContent(string.Format("http://css/www1/cmweb/vobepoch.plx?Action=Query&VOB={0}", vobName));
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            IEnumerable<string> headersAbbr = GetHeadersAbb(doc);
            int rowsCount = headersAbbr.Count();
            return new SyncVobData(vobName, headersAbbr, GetHeaders(doc), GetAllValues(doc, rowsCount), GetVobs(doc));
        }

        private string GetContent(string url)
        {
            using (StreamReader reader = new StreamReader(WebRequest.Create(url).GetResponse().GetResponseStream(), Encoding.Default))
            {
                return reader.ReadToEnd();
            }
        }

        private IEnumerable<string> GetVobs(HtmlDocument doc)
        {
            return doc.CreateNavigator()
                .Select("//option[@value]")
                .Cast<XPathNavigator>()
                .Select(h => h.GetAttribute("value", string.Empty));
        }

        private IEnumerable<string> GetHeadersAbb(HtmlDocument doc)
        {
            return doc.CreateNavigator()
                .Select("/html[1]/body[1]/table[2]/tbody[1]/tr[1]/th")
                .Cast<XPathNavigator>()
                .Skip(1)
                .Select(h => h.InnerXml);
        }
        private IEnumerable<string> GetHeaders(HtmlDocument doc)
        {
            return doc.CreateNavigator()
                .Select("//tr/th[@*]")
                .Cast<XPathNavigator>()
                .Skip(1)
                .Select(h => h.InnerXml);
        }
        private IEnumerable<IEnumerable<string>> GetAllValues(HtmlDocument doc, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                int rowNumber = i + 1;
                yield return doc.CreateNavigator()
                    .Select(string.Format("//tbody/tr[{0}]/td", rowNumber + 1))
                    .Cast<XPathNavigator>()
                    .Select(h => 
                        h.InnerXml.StartsWith("<b>", StringComparison.InvariantCultureIgnoreCase) 
                            ? h.SelectSingleNode(string.Format("//tbody/tr[{0}]/td/b/font", rowNumber + 1)).InnerXml
                            : h.InnerXml);
            }
        }
    }
}