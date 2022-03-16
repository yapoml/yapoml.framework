using System.Collections.Generic;

namespace Yapoml.Generation.Parsers.Yaml.Pocos
{
    public class Component
    {
        public string Name { get; set; }

        public By By { get; set; }

        public Dictionary<string, Component> Components { get; set; }
    }
}
