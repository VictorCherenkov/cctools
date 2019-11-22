namespace CCTools.Model
{
    public interface ICmdRunnerFactory
    {
        ICmdRunner CreateRunnerGetHostViews(string hostName, ILogger logger);
        ICmdRunner CreateUpdateRunner(string view, ILogger logger);
        ICmdRunner CreateCleanAndDebugMakeRunner(string makeDir, ILogger logger);
        ICmdRunner CreateCleanAndReleaseMakeRunner(string makeDir, ILogger logger);
        ICmdRunner CreateStopEchoRunner(ILogger logger);
        ICmdRunner CreateStartEchoRunner(ILogger logger);
        ICmdRunner CreateDebugMakeRunner(string makeDir, ILogger logger);
        ICmdRunner CreateReleaseMakeRunner(string makeDir, ILogger logger);
        ICmdRunner CreateRunnerCreateView(string branchName, ILogger logger);
        ICmdRunner CreateRunnerCreateBranch(string branchName, string vobName, ILogger logger);
    }
}