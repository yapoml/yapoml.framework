using System.IO;
using Yapoml.Parsers.Yaml;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Parsers
{
    public class Parser : IParser
    {
        public Page ParsePage(string filePath)
        {
            var content = File.ReadAllText(filePath);

            var model = new YamlParser().Parse<Page>(content);

            return model;
        }

        public Component ParseComponent(string filePath)
        {
            var content = File.ReadAllText(filePath);

            var model = new YamlParser().Parse<Component>(content);

            return model;
        }
    }
}
