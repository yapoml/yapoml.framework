using System.Collections.Generic;
using System.Text.RegularExpressions;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Framework.Workspace
{
    public class PageContext
    {
        public PageContext(string name, WorkspaceContext workspace, SpaceContext space, Page pageModel)
        {
            Name = NormalizeName(name);

            if (space != null)
            {
                Namespace = $"{space.Namespace}";
            }
            else
            {
                Namespace = $"{workspace.RootNamespace}";
            }

            ParentSpace = space;

            if (pageModel.Url != null)
            {
                Url = UrlContext.FromUrl(pageModel.Url);
            }

            if (pageModel.Components != null)
            {
                foreach (var component in pageModel.Components)
                {
                    Components.Add(new ComponentContext(component.Name, workspace, space, component));
                }
            }
        }

        public string Name { get; }

        public string Namespace { get; }

        public SpaceContext ParentSpace { get; }

        public IList<ComponentContext> Components { get; } = new List<ComponentContext>();

        public PageContext BasePage { get; set; }

        public UrlContext Url { get; }

        private string NormalizeName(string name)
        {
            return name.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        }

        public class UrlContext
        {
            public UrlContext(string path, IList<string> queryParams)
            {
                Path = path;
                Params = queryParams;

                Segments = ParseSegments(path);
            }

            public string Path { get; }

            public IList<string> Segments { get; }

            public IList<string> Params { get; }

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
                List<string> queryParams = null;

                if (urlModel.Params != null)
                {
                    queryParams = new List<string>();

                    foreach (var param in urlModel.Params)
                    {
                        queryParams.Add(param);
                    }
                }

                return new UrlContext(urlModel.Path, queryParams);
            }
        }
    }
}
