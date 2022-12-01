namespace Yapoml.Framework.Workspace.Parsers.Yaml.Pocos
{
    public class By
    {
        public ByMethod Method { get; set; }

        public string Value { get; set; }

        public enum ByMethod
        {
            None,
            XPath,
            Css,
            Id
        }

        public Region Region { get; set; }
    }
}
