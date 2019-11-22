namespace CCTools.Dll.Model
{
    public class LsViewsInfo
    {
        public ViewsBaseDirectory BaseDirectory { get; private set; }
        public LsViewInfo[] Views { get; private set; }

        public LsViewsInfo(ViewsBaseDirectory baseDirectory, LsViewInfo[] views)
        {
            BaseDirectory = baseDirectory;
            Views = views;
        }
    }
}