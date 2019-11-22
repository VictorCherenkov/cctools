using System;

namespace CCTools.View
{
    internal interface ILoggerControl
    {
        void Clear();
        void Flush();
        double BufferDelayMsec { get; set; }
        void SetText(string text);
        void AppendLine(string arg);
        void Invoke(Action<string> action, string line);
        void SetText(string[] lines);
    }
}