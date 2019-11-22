using System;
using CCTools.CmdRun;

namespace CCTools.Model
{
    public class CmdRunnerFactory : ICmdRunnerFactory
    {
        public ICmdRunner CreateRunnerGetHostViews(string hostName, ILogger logger)
        {
            return CreateRunnerLogAll(logger, CmdSamples.GetViews());
        }
        public ICmdRunner CreateUpdateRunner(string viewPath, ILogger logger)
        {
            return CreateRunnerLogAll(logger, CmdSamples.Update(viewPath));
        }
        public ICmdRunner CreateCleanAndDebugMakeRunner(string makeDir, ILogger logger)
        {
            return CreateRunnerLogAll(logger, CmdSamples.CleanAndMakeDebug(makeDir));
        }

        public ICmdRunner CreateDebugMakeRunner(string makeDir, ILogger logger)
        {
            return CreateRunnerLogAll(logger, new Cmd("cmd", string.Format(@"cd {0}", makeDir), "omake debug", "omake mkwebsvcs"));
        }

        public ICmdRunner CreateReleaseMakeRunner(string makeDir, ILogger logger)
        {
            return CreateRunnerLogAll(logger, new Cmd("cmd", string.Format(@"cd {0}", makeDir), "omake", "omake mkwebsvcs"));
        }

        public ICmdRunner CreateRunnerCreateView(string branchName, ILogger logger)
        {
            return CreateRunnerLogAll(logger, new Cmd("cleartool", string.Format(@"mkview -snapshot ""\\vcherenkov\Views\SR\{0}""", branchName)));
        }

        public ICmdRunner CreateRunnerCreateBranch(string branchName, string vobName, ILogger logger)
        {            
            return CreateRunnerLogAll(logger, new Cmd("cleartool", string.Format(@"mkbrtype -c .  {0}@\{1}", branchName, vobName)));
        }

        public ICmdRunner CreateCleanAndReleaseMakeRunner(string makeDir, ILogger logger)
        {
            return CreateRunnerLogAll(logger, new Cmd("cmd", string.Format(@"cd {0}", makeDir), "omake distclean", "omake", "omake mkwebsvcs"));
        }

        public ICmdRunner CreateStopEchoRunner(ILogger logger)
        {
            return CreateRunnerLogAll(logger, new Cmd(
                                                  "cmd",
                                                  "echo off",
                                                  "echo \"Resetting the ECHO service if a non-standard shut down was detected\"",
                                                  "echo \"Stop ECHO\"",
                                                  "net stop \"ECHO Service\"",
                                                  "del \"C:\\Program Files\\ECHO\\CHSvcRunning\""));
        }

        public ICmdRunner CreateStartEchoRunner(ILogger logger)
        {
            return CreateRunnerLogAll(logger, new Cmd("cmd", "echo \"Start ECHO\"", "net start \"ECHO Service\""));
        }

        private static ICmdRunner CreateRunnerLogAll(ILogger writer, params Cmd[] commands)
        {
            return new CmdRunner(LoggerRange.CreateLogAll(writer), commands);
        }        
    }

    public class CmdSamples
    {
        public static Cmd GetViews()
        {
            return new Cmd("cleartool", "lsview");
        }
        public static Cmd Update(string viewPath)
        {
            return new Cmd("cleartool", string.Format(@"cd {0}", viewPath), "update", "Y");
        }
        public static Cmd CleanAndMakeDebug(string makeDir)
        {
            return new Cmd("cmd", string.Format(@"cd {0}", makeDir), "omake distclean", "omake debug", "omake mkwebsvcs");
        }
        public static Cmd MakeDebug(string makeDir)
        {
            return new Cmd("cmd", string.Format(@"cd {0}", makeDir), "omake debug", "omake mkwebsvcs");
        }

    }
}