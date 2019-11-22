namespace CCTools.Dll.Model
{
    public class LsViewInfo
    {
        public string ServerAccessPath { get; private set; }
        public string Uuid { get; private set; }

        public LsViewInfo(string serverAccessPath, string uuid)
        {
            ServerAccessPath = serverAccessPath;
            Uuid = uuid;
        }
    }
}