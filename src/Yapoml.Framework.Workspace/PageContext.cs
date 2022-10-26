using System.Collections.Generic;
using System.Linq;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Workspace
{
    public class PageContext
    {
        private readonly Page _page;

        public PageContext(WorkspaceContext workspace, SpaceContext space, Page pageModel, string relativeFilePath)
        {
            Workspace = workspace;
            ParentSpace = space;

            _page = pageModel;
            RelativeFilePath = relativeFilePath;
        }

        public WorkspaceContext Workspace { get; }

        public string RelativeFilePath { get; }

        private string _name;
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    _name = Workspace.NameNormalizer.Normalize(_page.Name);
                }

                return _name;
            }
        }

        private string _namespace;
        public string Namespace
        {
            get
            {
                if (_namespace is null)
                {
                    if (ParentSpace != null)
                    {
                        _namespace = ParentSpace.Namespace;
                    }
                    else
                    {
                        _namespace = Workspace.RootNamespace;
                    }
                }

                return _namespace;
            }
        }

        public SpaceContext ParentSpace { get; }

        private IReadOnlyList<ComponentContext> _components;
        public IReadOnlyList<ComponentContext> Components
        {
            get
            {
                if (_components is null)
                {
                    _components = _page.Components?.Select(c => new ComponentContext(Workspace, ParentSpace, this, null, c)).ToList();
                }

                return _components;
            }
        }

        private string _basePageName;
        public string BasePageName
        {
            get
            {
                if (_basePageName is null)
                {
                    if (_page.BasePage != null)
                    {
                        _basePageName = Workspace.NameNormalizer.Normalize(_page.BasePage);
                    }
                }

                return _basePageName;
            }
        }

        public PageContext BasePage { get; set; }

        private UrlContext _url;
        public UrlContext Url
        {
            get
            {
                if (_url is null)
                {
                    if (_page.Url != null)
                    {
                        _url = UrlContext.FromUrl(_page.Url);
                    }
                }

                return _url;
            }
        }

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
