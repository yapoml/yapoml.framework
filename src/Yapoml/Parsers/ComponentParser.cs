using System.IO;
using Yapoml.Parsers.Yaml;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Parsers
{
    public class ComponentParser : IComponentParser
    {
        public Component Parse(string fileName)
        {
            var content = File.ReadAllText(fileName);

            var component = new YamlParser().Parse<Component>(content);

            return component;
        }
    }
}
