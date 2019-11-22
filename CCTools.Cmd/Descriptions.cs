namespace CCTools.Cmd
{
    internal static class Descriptions
    {
        public const string ConfigSpec = "Shows current config spec";
        public const string FullUpdate = "Fully updates the view using latest config spec.";
        public const string ShowChangelist = "Shows changed files (like brtool does)";
        public const string NewSr = "Creates new SR and new view or uses current one. E.g.: c sr jsr594615 CGACoreGUI 2.75 cgacoregui_2.75_1";
        public const string MergeToRemote = "Merges branch to remote. If remote branch is not specified - main is used.";
        public const string MergeToRemoteSlow = "Merges branch to remote. If remote branch is not specified - main is used.";
        public const string MergeFromRemote = "Merges to branch from remote.";
        public const string OpenBrtool = "Opens brtool";
        public const string ShowVersionTree = "Shows version tree for the file even if you are inside the git repository.";
        public const string NewBuild = "Creates bld view or uses current one.";
        public const string ApplyCs = "Applies current config spec";
    }
}