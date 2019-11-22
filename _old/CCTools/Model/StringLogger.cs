using System.Text;

namespace CCTools.Model
{
    public class StringLogger : ILogger
    {
        private readonly StringBuilder m_sb;

        public StringLogger()
        {
            m_sb = new StringBuilder();
        }

        public string OutputString
        {
            get { return m_sb.ToString(); }
        }

        public void WriteLine(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return;
            }
            m_sb.AppendLine(s);
        }
    }
}