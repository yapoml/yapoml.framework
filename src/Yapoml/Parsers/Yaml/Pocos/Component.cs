using YamlDotNet.Serialization;

namespace Yapoml.Parsers.Yaml.Pocos
{
    public class Component
    {
        public string Name { get; set; }

        [YamlMember(Alias = "by")]
        public By By { get; set; }
    }
}
