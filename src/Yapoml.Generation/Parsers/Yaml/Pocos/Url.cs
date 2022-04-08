using System.Collections.Generic;

namespace Yapoml.Generation.Parsers.Yaml.Pocos
{
    public class Url
    {
        public string Path { get; set; }

        public IList<string> Params { get; set; }
    }
}
