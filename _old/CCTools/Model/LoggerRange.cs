using System;

namespace CCTools.Model
{
    /// <summary>
    /// This logger states and jumps are following: Disabled->Enabled->Done.
    /// </summary>
    public class LoggerRange
    {
        private enum State
        {
            Disabled,
            Enabled,
            Done
        }

        private State m_state;
        private readonly Func<string, bool> m_isStart;
        private readonly Func<string, bool> m_isFinish;
        private readonly ILogger m_logger;

        private LoggerRange(ILogger logger,
                            Func<string, bool> logStartHandler,
                            Func<string, bool> logFinishHandler,
                            State initialState)
        {
            m_logger = logger;
            m_isStart = logStartHandler;
            m_isFinish = logFinishHandler;
            m_state = initialState;
        }

        public static LoggerRange CreateLogFromTo(ILogger logger, Func<string, bool> from, Func<string, bool> to)
        {
            return new LoggerRange(logger, from, to, State.Disabled);
        }
        public static LoggerRange CreateLogFrom(ILogger logger, Func<string, bool> from)
        {
            return new LoggerRange(logger, from, delegate { return false; }, State.Disabled);
        }
        public static LoggerRange CreateLogTo(ILogger logger, Func<string, bool> to)
        {
            return new LoggerRange(logger, delegate { return true; }, to, State.Enabled);
        }
        public static LoggerRange CreateLogAll(ILogger logger)
        {
            return new LoggerRange(logger, delegate { return true; }, delegate { return false; }, State.Enabled);
        }
        public void LogData(string s)
        {
            if (string.IsNullOrEmpty(s) ||
                m_state == State.Done)
            {
                return;
            }

            bool isEnable = m_state == State.Disabled && m_isStart(s);
            bool isSetDone = m_state == State.Enabled && m_isFinish(s);
            bool isLog = m_state == State.Enabled && !m_isFinish(s);

            if (isEnable)
            {
                m_state = State.Enabled;
            }
            else if (isSetDone)
            {
                m_state = State.Done;
            }
            else if (isLog)
            {
                m_logger.WriteLine(s);
            }
        }
    }
}