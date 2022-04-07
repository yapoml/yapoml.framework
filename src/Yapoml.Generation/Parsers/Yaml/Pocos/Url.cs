using System.Collections.Generic;

namespace Yapoml.Generation.Parsers.Yaml.Pocos
{
    public class Url
    {
        public string Path { get; set; }

        public IList<QueryParam> QueryParams { get; set; }

        public class QueryParam
        {
            public string Name { get; set; }

            public bool IsOptional { get; set; } = true;
        }
    }
}
