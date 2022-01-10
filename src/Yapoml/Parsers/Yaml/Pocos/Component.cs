using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Yapoml.Parsers.Yaml.Pocos
{
    public class Component
    {
        public string Name { get; set; }

        [YamlMember(Alias = "by")]
        public By By { get; set; }

        [YamlMember(Alias = "ya")]
        public Dictionary<string, Component> Components { get; set; }
    }
}
