namespace CCTools.Dll.Model
{
    public class ProductId
    {
        public string Name { get; private set; }
        public ProductId(string name)
        {
            Name = name;
        }
    }
}