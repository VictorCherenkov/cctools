namespace CCTools.Dll.Model
{
    public class View
    {
        public static View Sr(ViewRoot viewRoot, ConfigSpec configSpec, ProductId productId, TargetVersion targetVersion, SrBranchName srBranchName)
        {
            return new View(viewRoot, configSpec, productId, targetVersion, srBranchName, null, Model.ViewType.Sr);
        }
        public static View Bld(ViewRoot viewRoot, ConfigSpec configSpec, ProductId productId, TargetVersion targetVersion, string build)
        {
            return new View(viewRoot, configSpec, productId, targetVersion, null, build, Model.ViewType.Build);
        }
        public static View Unknown(ViewRoot viewRoot, ConfigSpec configSpec)
        {
            return new View(viewRoot, configSpec, null, null, null, null, null);
        }

        public ViewRoot ViewRoot { get; }
        public ConfigSpec ConfigSpec { get; }
        public ProductId ProductId { get; }
        public TargetVersion TargetVersion { get; }
        public SrBranchName SrBranchName { get; }
        public string Build { get; }
        public ViewType? ViewType { get; }

        private View(ViewRoot viewRoot, ConfigSpec configSpec, ProductId productId, TargetVersion targetVersion, SrBranchName srBranchName, string build, ViewType? viewType)
        {
            ViewRoot = viewRoot;
            ConfigSpec = configSpec;
            ProductId = productId;
            TargetVersion = targetVersion;
            SrBranchName = srBranchName;
            Build = build;
            ViewType = viewType;
        }

        public string DisplayText => ViewType == null ? $"Unknown    {ViewRoot.LocalPath}"
            : ViewType.Value == Model.ViewType.Sr ? $"{ProductId.Name} {TargetVersion.Value}    {ViewRoot.LocalPath}    ({SrBranchName.Value})"
            : ViewType.Value == Model.ViewType.Build ? $"{ProductId?.Name ?? "?"} {TargetVersion?.Value ?? "?"}    {ViewRoot.LocalPath}    ({Build ?? "?"})"
            : null;
    }
}