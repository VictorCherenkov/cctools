using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CCTools.Model
{
    public class SyncVobData
    {
        private readonly string m_vobName;
        private readonly IEnumerable<string> m_headersAbbr;
        private readonly IEnumerable<string> m_headers;        
        private readonly IEnumerable<IEnumerable<string>> m_values;
        private readonly IEnumerable<string> m_vobs;

        public SyncVobData(string vobName,
            IEnumerable<string> headersAbbr, 
            IEnumerable<string> headers, 
            IEnumerable<IEnumerable<string>> values, 
            IEnumerable<string> vobs)
        {
            m_vobName = vobName;
            m_headersAbbr = headersAbbr;
            m_headers = headers;
            m_values = values;
            m_vobs = vobs;
        }

        public string VobName
        {
            get { return m_vobName; }
        }

        public IEnumerable<string> HeadersAbbr
        {
            get { return m_headersAbbr; }
        }

        public IEnumerable<string> Headers
        {
            get { return m_headers; }
        }

        public IEnumerable<IEnumerable<string>> Values
        {
            get { return m_values; }
        }

        public IEnumerable<string> Vobs
        {
            get { return m_vobs; }
        }

        public DataTable AsTable()
        {
            //Use SelectMany instead of this complex syntax
            IEnumerable<IEnumerable<string>> allValues = m_values
                .Select((v, i) => new { Values = v, Index = i })
                .Join(m_headers.Select((h, i) => new { Header = h, Index = i }),
                      v => v.Index,
                      h => h.Index,
                      (v, h) => new[] { h.Header }.Concat(v.Values));
            return GetSyncVobTable(m_headersAbbr, allValues);
        }

        private DataTable GetSyncVobTable(IEnumerable<string> headersAbbr, IEnumerable<IEnumerable<string>> rows)
        {
            DataTable dt = new DataTable("SyncVob");
            dt.Columns.Add(new DataColumn("#"));
            dt.Columns.AddRange(headersAbbr.Select(s => new DataColumn(s)).ToArray());
            foreach (var row in rows)
            {
                dt.Rows.Add(row.ToArray());
            }
            return dt;
        }
    }
}