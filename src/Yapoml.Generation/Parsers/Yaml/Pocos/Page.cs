using System.Collections.Generic;

namespace Yapoml.Generation.Parsers.Yaml.Pocos
{
    public class Page
    {
        public IList<Component> Components { get; set; }

        public string BasePage { get; set; }

        public Url Url { get; set; }
    }
}
