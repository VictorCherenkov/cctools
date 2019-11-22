namespace CCTools.Dll.Model
{
    public class ViewsBaseDirectory
    {
        public string LocalPath { get; }
        public string NetworkPath { get; }
        public ViewsBaseDirectory(string localPath, string networkPath)
        {
            LocalPath = localPath;
            NetworkPath = networkPath;
        }
    }
}