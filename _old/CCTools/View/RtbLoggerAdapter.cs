using System.Windows.Forms;

namespace CCTools.View
{
    internal class RtbLoggerAdapter : ILogger
    {
        private readonly ILoggerControl m_rtb;

        public RtbLoggerAdapter(ILoggerControl loggerControl)
        {
            m_rtb = loggerControl;
        }

        public void WriteLine(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return;
            }
            m_rtb.Invoke(m_rtb.AppendLine, line);
            Application.DoEvents();
        }
    }
}