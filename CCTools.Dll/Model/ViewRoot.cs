namespace CCTools.Dll.Model
{
    public class ViewRoot
    {
        public string LocalPath { get; private set; }
        public string ServerAccessPath { get; private set; }
        public ViewRoot(string localPath, string serverAccessPath)
        {
            LocalPath = localPath;
            ServerAccessPath = serverAccessPath;
        }
    }
}