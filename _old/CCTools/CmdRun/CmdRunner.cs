using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CCTools.Model;

namespace CCTools.CmdRun
{
    public class CmdRunner : ICmdRunner
    {
        private const string c_exitCmd = "exit";

        private readonly LoggerRange m_logger;
        private readonly Cmd[] m_commands;

        internal CmdRunner(LoggerRange logger, params Cmd[] commands)
        {
            m_logger = logger;
            m_commands = commands;
        }

        public void Run()
        {
            Thread t = new Thread(ThreadMethod);
            t.IsBackground = true;
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            while (t.IsAlive)
            {
                Application.DoEvents();
            }
        }
        private void ThreadMethod()
        {
            foreach (Cmd command in m_commands)
            {
                Execute(command);
            }
            
        }
        private void Execute(Cmd command)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = command.ProcessName;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.OutputDataReceived += OutputDataReceived;
            proc.Start();
            proc.PriorityClass = ProcessPriorityClass.Idle;
            proc.BeginOutputReadLine();

            foreach (string cmd in command.ProcessArguments)
            {
                proc.StandardInput.WriteLine(cmd);
            }
            proc.StandardInput.WriteLine(c_exitCmd);
            proc.WaitForExit();
        }
        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            m_logger.LogData(e.Data);
        }
    }
}