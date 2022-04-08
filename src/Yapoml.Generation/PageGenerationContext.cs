using System.Collections.Generic;
using System.Text.RegularExpressions;
using Yapoml.Generation.Parsers.Yaml.Pocos;

namespace Yapoml.Generation
{
    public class PageGenerationContext
    {
        public PageGenerationContext(string name, GlobalGenerationContext globalContext, SpaceGenerationContext spaceContext, Page pageModel)
        {
            Name = NormalizeName(name);

            if (spaceContext != null)
            {
                Namespace = $"{spaceContext.Namespace}";
            }
            else
            {
                Namespace = $"{globalContext.RootNamespace}";
            }

            ParentContext = spaceContext;

            if (pageModel.Url != null)
            {
                Url = UrlContext.FromUrl(pageModel.Url);
            }

            if (pageModel.Components != null)
            {
                foreach (var component in pageModel.Components)
                {
                    Components.Add(new ComponentGenerationContext(component.Name, globalContext, spaceContext, component));
                }
            }
        }

        public string Name { get; }

        public string Namespace { get; }

        public SpaceGenerationContext ParentContext { get; }

        public IList<ComponentGenerationContext> Components { get; } = new List<ComponentGenerationContext>();

        public PageGenerationContext BasePageContext { get; set; }

        public UrlContext Url { get; }

        private string NormalizeName(string name)
        {
            return name.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        }

        public class UrlContext
        {
            public UrlContext(string path, IList<QueryParamContext> queryParams)
            {
                Path = path;
                QueryParams = queryParams;

                Segments = ParseSegments(path);
            }

            public string Path { get; }

            public IList<string> Segments { get; }

            public IList<QueryParamContext> QueryParams { get; }

            public class QueryParamContext
            {
                public QueryParamContext(string name, bool isOptional)
                {
                    Name = name;
                    IsOptional = isOptional;
                }

                public string Name { get; }

                public bool IsOptional { get; }
            }

            private IList<string> ParseSegments(string path)
            {
                // todo: performance, don't use regex for simple searching for {segment}
                var matches = Regex.Matches(path, "{(.*?)}");

                List<string> segments = null;

                if (matches.Count > 0)
                {
                    segments = new List<string>();

                    foreach (Match match in matches)
                    {
                        segments.Add(match.Groups[1].Value);
                    }
                }

                return segments;
            }

            public static UrlContext FromUrl(Url urlModel)
            {
                List<QueryParamContext> queryParams = null;

                if (urlModel.QueryParams != null)
                {
                    queryParams = new List<QueryParamContext>();

                    foreach (var param in urlModel.QueryParams)
                    {
                        queryParams.Add(new QueryParamContext(param.Name, param.IsOptional));
                    }
                }

                return new UrlContext(urlModel.Path, queryParams);
            }
        }
    }
}
