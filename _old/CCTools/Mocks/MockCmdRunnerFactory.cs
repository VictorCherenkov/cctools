using System.IO;
using CCTools.Model;

namespace CCTools.Mocks
{
    public class MockCmdRunnerFactory : ICmdRunnerFactory
    {
        public ICmdRunner CreateRunnerGetHostViews(string hostName, ILogger logger)
        {
            logger.WriteLine("sr\\cgafesr666\\ sr\\cgafesr666\\ ");
            if (!Directory.Exists("sr\\cgafesr666"))
            {
                Directory.CreateDirectory("sr\\cgafesr666");
            }
            return new MockCmdRunner();
        }

        public ICmdRunner CreateUpdateRunner(string view, ILogger logger)
        {
            return new MockCmdRunner();
        }

        public ICmdRunner CreateCleanAndDebugMakeRunner(string makeDir, ILogger logger)
        {
            return new MockCmdRunner();
        }

        public ICmdRunner CreateCleanAndReleaseMakeRunner(string makeDir, ILogger logger)
        {
            return new MockCmdRunner();
        }

        public ICmdRunner CreateStopEchoRunner(ILogger logger)
        {
            return new MockCmdRunner();
        }

        public ICmdRunner CreateStartEchoRunner(ILogger logger)
        {
            return new MockCmdRunner();
        }

        public ICmdRunner CreateDebugMakeRunner(string makeDir, ILogger logger)
        {
            return new MockCmdRunner();
        }

        public ICmdRunner CreateReleaseMakeRunner(string makeDir, ILogger logger)
        {
            return new MockCmdRunner();
        }

        public ICmdRunner CreateRunnerCreateView(string branchName, ILogger logger)
        {
            return new MockCmdRunner();
        }

        public ICmdRunner CreateRunnerCreateBranch(string branchName, string vobName, ILogger logger)
        {
            return new MockCmdRunner();
        }

        private class MockCmdRunner : ICmdRunner
        {
            void ICmdRunner.Run()
            {
            }
        }
    }
}