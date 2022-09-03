using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers.Yaml.Pocos
{
    public class Page
    {
        public string Name { get; set; }

        public IList<Component> Components { get; set; }

        public string BasePage { get; set; }

        public Url Url { get; set; }
    }
}
