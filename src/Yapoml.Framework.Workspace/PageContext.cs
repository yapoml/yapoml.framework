using System.Collections.Generic;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Workspace
{
    public class PageContext
    {
        public PageContext(string name, WorkspaceContext workspace, SpaceContext space, Page pageModel, INameNormalizer nameNormalizer)
        {
            Name = nameNormalizer.Normalize(name);

            Workspace = workspace;

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

            if (pageModel.BasePage != null)
            {
                BasePageName = nameNormalizer.Normalize(pageModel.BasePage);
            }

            if (pageModel.Components != null)
            {
                foreach (var component in pageModel.Components)
                {
                    Components.Add(new ComponentContext(workspace, space, this, null, component, nameNormalizer));
                }
            }
        }

        public WorkspaceContext Workspace { get; }

        public string Name { get; }

        public string Namespace { get; }

        public SpaceContext ParentSpace { get; }

        public IList<ComponentContext> Components { get; } = new List<ComponentContext>();

        public string BasePageName { get; }

        public PageContext BasePage { get; set; }

        public UrlContext Url { get; }

        public class UrlContext
        {
            public UrlContext(string path, IList<string> queryParams)
            {
                Path = path;
                Params = queryParams;

                Segments = SegmentsParser.ParseSegments(path);
            }

            public string Path { get; }

            public IList<string> Segments { get; }

            public IList<string> Params { get; }

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
