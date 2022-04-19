using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers.Yaml.Pocos
{
    public class Url
    {
        public string Path { get; set; }

        public IList<string> Params { get; set; }
    }
}
