using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers.Yaml.Pocos
{
    public class Component
    {
        public string Name { get; set; }

        public By By { get; set; }

        public string Ref { get; set; }

        public string BaseComponent { get; set; }

        public IList<Component> Components { get; set; }
    }
}
