using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CCTools.View
{
    public class ExtendedRichTextBox : RichTextBox, ILoggerControl
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        private const int c_DefaultBufDelayMs = 5;

        private const int WM_VSCROLL = 277;
        private const int SB_BOTTOM = 7;

        private double m_bufferDelayMsec = 5;
        private DateTime m_lastFlushTime = DateTime.MinValue;
        private StringBuilder m_buffer = new StringBuilder();

        public ExtendedRichTextBox()
        {
            m_bufferDelayMsec = c_DefaultBufDelayMs;
        }

        public new void AppendText(string text)
        {
            m_buffer.Append(text);
            if (IsFlushBuffer())
            {
                Flush();
            }
        }
        public void Flush()
        {
            Invoke(new Action(delegate
                                 {
                                     base.AppendText(m_buffer.ToString());
                                     m_buffer = new StringBuilder();
                                     m_lastFlushTime = DateTime.Now;
                                     SelectionStart = Text.Length;
                                     SelectionLength = 0;
                                     ScrollToBottom();
                                 }));
        }

        public void AppendLine(string line)
        {
            AppendText(line + Environment.NewLine);
        }

        public void Invoke(Action<string> action, string line)
        {
            base.Invoke(action, line);
        }

        public double BufferDelayMsec
        {
            get { return m_bufferDelayMsec; }
            set { m_bufferDelayMsec = value; }
        }

        public void SetText(string text)
        {
            Text = text;
        }

        public void SetText(string[] lines)
        {
            Lines = lines;
        }

        public void ScrollToBottom()
        {
            SendMessage(Handle, WM_VSCROLL, new IntPtr(SB_BOTTOM), new IntPtr(0));
        }        
        private bool IsFlushBuffer()
        {
            return (DateTime.Now - m_lastFlushTime).TotalMilliseconds > m_bufferDelayMsec;
        }
    }    
}