using Yapoml.Parsers.Yaml;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Parsers
{
    public class ComponentParser : IComponentParser
    {
        public Component Parse(string fileName)
        {
            var component = new YamlParser().Parse<Component>(fileName);

            return component;
        }
    }
}
