namespace Yapoml.Parsers.Yaml.Pocos
{
    public class By
    {
        public ByMethod Method { get; set; }

        public string Value { get; set; }

        public enum ByMethod
        {
            XPath,
            Css
        }
    }
}
