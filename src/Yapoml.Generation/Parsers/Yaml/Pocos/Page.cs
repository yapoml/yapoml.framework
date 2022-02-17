using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Yapoml.Generation.Parsers.Yaml.Pocos
{
    public class Page
    {
        [YamlMember(Alias = "ya")]
        public Dictionary<string, Component> Components { get; set; }
    }
}
