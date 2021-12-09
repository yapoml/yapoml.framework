using System.IO;
using Yapoml.Parsers.Yaml;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Parsers
{
    public class Parser : IParser
    {
        public Page ParsePage(string fileName)
        {
            var content = File.ReadAllText(fileName);

            var model = new YamlParser().Parse<Page>(content);

            return model;
        }

        public Component ParseComponent(string fileName)
        {
            var content = File.ReadAllText(fileName);

            var model = new YamlParser().Parse<Component>(content);

            return model;
        }
    }
}
