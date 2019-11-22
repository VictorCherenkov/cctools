namespace CCTools.CmdRun
{
    public class Cmd
    {
        private readonly string m_processName;
        private readonly string[] m_processArguments;

        public Cmd(string processName, params string[] commands)
        {
            m_processName = processName;
            m_processArguments = commands;
        }

        public string ProcessName
        {
            get { return m_processName; }
        }

        public string[] ProcessArguments
        {
            get { return m_processArguments; }
        }
    }
}