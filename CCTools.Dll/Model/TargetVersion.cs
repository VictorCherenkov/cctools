namespace CCTools.Dll.Model
{
    public class TargetVersion
    {
        public string Value { get; private set; }

        public TargetVersion(string value)
        {
            Value = value;
        }
    }
}