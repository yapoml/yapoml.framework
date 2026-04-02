using System.Collections.Generic;
using System.Linq;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Workspace;

/// <summary>
/// Represents a page definition within the workspace, containing its components, URL, and inheritance information.
/// </summary>
public class PageContext
{
    private readonly Page _page;

    /// <summary>
    /// Initializes a new instance of the <see cref="PageContext"/> class.
    /// </summary>
    /// <param name="workspace">The workspace this page belongs to.</param>
    /// <param name="space">The space this page belongs to, or <see langword="null"/> for top-level pages.</param>
    /// <param name="pageModel">The parsed page model.</param>
    /// <param name="relativeFilePath">The file path relative to the workspace root.</param>
    internal PageContext(WorkspaceContext workspace, SpaceContext space, Page pageModel, string relativeFilePath)
    {
        Workspace = workspace;
        ParentSpace = space;

        _page = pageModel;
        RelativeFilePath = relativeFilePath;
    }

    /// <summary>
    /// Gets the workspace this page belongs to.
    /// </summary>
    public WorkspaceContext Workspace { get; }

    /// <summary>
    /// Gets the file path relative to the workspace root.
    /// </summary>
    public string RelativeFilePath { get; }

    private string _name;

    /// <summary>
    /// Gets the normalized name of the page.
    /// </summary>
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

    /// <summary>
    /// Gets the fully qualified namespace of this page.
    /// </summary>
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

    /// <summary>
    /// Gets the space this page belongs to, or <see langword="null"/> for top-level pages.
    /// </summary>
    public SpaceContext ParentSpace { get; }

    private IReadOnlyList<ComponentContext> _components;

    /// <summary>
    /// Gets the components defined within this page.
    /// </summary>
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

    /// <summary>
    /// Gets the normalized name of the base page this page inherits from, or <see langword="null"/> if none.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the resolved base page context, or <see langword="null"/> if this page has no base.
    /// </summary>
    public PageContext BasePage { get; internal set; }

    private UrlContext _url;

    /// <summary>
    /// Gets the URL context for this page, or <see langword="null"/> if no URL is defined.
    /// </summary>
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

    /// <summary>
    /// Represents the URL configuration of a page, including path segments and query parameters.
    /// </summary>
    public class UrlContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlContext"/> class.
        /// </summary>
        /// <param name="path">The URL path template.</param>
        /// <param name="queryParams">The list of query parameter names.</param>
        public UrlContext(string path, IList<string> queryParams)
        {
            Path = path;
            Params = queryParams;

            Segments = SegmentsParser.ParseSegments(path);
        }

        /// <summary>
        /// Gets the URL path template.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the parameterized segments extracted from the path template.
        /// </summary>
        public IList<string> Segments { get; }

        /// <summary>
        /// Gets the list of query parameter names.
        /// </summary>
        public IList<string> Params { get; }

        /// <summary>
        /// Creates a <see cref="UrlContext"/> from a parsed <see cref="Url"/> model.
        /// </summary>
        /// <param name="urlModel">The parsed URL model.</param>
        /// <returns>A new <see cref="UrlContext"/> instance.</returns>
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
