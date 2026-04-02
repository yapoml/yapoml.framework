using System.Collections.Generic;
using System.Linq;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Workspace;

/// <summary>
/// Represents a component definition within the workspace, including its locator, inheritance, and nested components.
/// </summary>
public class ComponentContext
{
    private readonly Component _component;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentContext"/> class.
    /// </summary>
    /// <param name="workspace">The workspace this component belongs to.</param>
    /// <param name="space">The space this component belongs to.</param>
    /// <param name="page">The page this component belongs to, or <see langword="null"/> for standalone components.</param>
    /// <param name="parentComponent">The parent component, or <see langword="null"/> for top-level components.</param>
    /// <param name="component">The parsed component model.</param>
    /// <param name="relativeFilePath">The file path relative to the workspace root.</param>
    internal ComponentContext(WorkspaceContext workspace, SpaceContext space, PageContext page, ComponentContext parentComponent, Component component, string relativeFilePath = null)
    {
        Workspace = workspace;
        Space = space;
        Page = page;
        ParentComponent = parentComponent;

        _component = component;
        RelativeFilePath = relativeFilePath;
    }

    /// <summary>
    /// Gets the workspace this component belongs to.
    /// </summary>
    public WorkspaceContext Workspace { get; }

    /// <summary>
    /// Gets the space this component belongs to.
    /// </summary>
    public SpaceContext Space { get; }

    /// <summary>
    /// Gets the page this component belongs to, or <see langword="null"/> for standalone components.
    /// </summary>
    public PageContext Page { get; }

    /// <summary>
    /// Gets the parent component, or <see langword="null"/> for top-level components.
    /// </summary>
    public ComponentContext ParentComponent { get; }

    private string _relativeFilePath;

    /// <summary>
    /// Gets or sets the file path relative to the workspace root.
    /// </summary>
    public string RelativeFilePath
    {
        get
        {
            if (_relativeFilePath is null)
            {
                if (ParentComponent != null)
                {
                    _relativeFilePath = ParentComponent.RelativeFilePath;
                }
                else
                {
                    _relativeFilePath = Page.RelativeFilePath;
                }
            }

            return _relativeFilePath;
        }
        internal set
        {
            _relativeFilePath = value;
        }
    }

    private string _name;

    /// <summary>
    /// Gets the normalized name of the component.
    /// </summary>
    public string Name
    {
        get
        {
            if (_name is null)
            {
                _name = Workspace.NameNormalizer.Normalize(_component.Name);
            }

            return _name;
        }
    }

    private string _originalName;

    /// <summary>
    /// Gets the original (non-normalized) name of the component as defined in the source file.
    /// </summary>
    public string OriginalName
    {
        get
        {
            if (_originalName is null)
            {
                _originalName = _component.Name;
            }

            return _originalName;
        }
    }

    private string _namespace;

    /// <summary>
    /// Gets the fully qualified namespace of this component.
    /// </summary>
    public string Namespace
    {
        get
        {
            if (_namespace is null)
            {
                if (ParentComponent != null)
                {
                    _namespace = $"{ParentComponent.Namespace}.{ParentComponent.Name}";
                }
                else if (Page != null)
                {
                    _namespace = $"{Page.Namespace}.{Page.Name}";
                }
                else if (Space != null)
                {
                    _namespace = Space.Namespace;
                }
                else
                {
                    _namespace = Workspace.RootNamespace;
                }
            }

            return _namespace;
        }
    }

    private ByContext _by;

    /// <summary>
    /// Gets or sets the locator strategy for finding this component, or <see langword="null"/> if not specified.
    /// </summary>
    public ByContext By
    {
        get
        {
            if (_by is null)
            {
                if (_component.By != null)
                {
                    _by = new ByContext(_component.By.Method, _component.By.Value, _component.By.Scope, new DefinitionSource(RelativeFilePath, _component.By.Region));
                }
            }

            return _by;
        }
        internal set
        {
            _by = value;
        }
    }

    private string _baseComponentName;

    /// <summary>
    /// Gets the normalized name of the base component this component inherits from, or <see langword="null"/> if none.
    /// </summary>
    public string BaseComponentName
    {
        get
        {
            if (_baseComponentName is null)
            {
                if (_component.BaseComponent != null)
                {
                    _baseComponentName = Workspace.NameNormalizer.Normalize(_component.BaseComponent);
                }
            }

            return _baseComponentName;
        }
    }

    /// <summary>
    /// Gets or sets the resolved base component context, or <see langword="null"/> if this component has no base.
    /// </summary>
    public ComponentContext BaseComponent { get; internal set; }

    private bool? _isPlural;

    /// <summary>
    /// Gets a value indicating whether this component's name is plural.
    /// </summary>
    public bool IsPlural
    {
        get
        {

            if (!_isPlural.HasValue)
            {
                _isPlural = new PluralizationService().IsPlural(Name);
            }

            return _isPlural.Value;
        }
    }

    private string _singularName;

    /// <summary>
    /// Gets the singular form of this component's name.
    /// </summary>
    public string SingularName
    {
        get
        {
            if (_singularName is null)
            {
                _singularName = new PluralizationService().Singularize(Name);
            }

            return _singularName;
        }
    }

    private IReadOnlyList<ComponentContext> _components;

    /// <summary>
    /// Gets the nested components defined within this component.
    /// </summary>
    public IReadOnlyList<ComponentContext> Components
    {
        get
        {
            if (_components is null)
            {
                if (_component.Components != null)
                {
                    _components = _component.Components.Select(c => new ComponentContext(Workspace, Space, Page, this, c)).ToList();
                }
            }

            return _components;
        }
    }

    /// <summary>
    /// Represents the locator strategy used to find a component.
    /// </summary>
    public class ByContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByContext"/> class.
        /// </summary>
        /// <param name="method">The locator method (e.g., XPath, Css).</param>
        /// <param name="value">The locator value.</param>
        /// <param name="scope">The scope in which to search for the element.</param>
        /// <param name="definitionSource">The source file and region where this locator is defined.</param>
        public ByContext(By.ByMethod method, string value, By.ByScope scope, DefinitionSource definitionSource)
        {
            Method = method;
            Value = value;
            Scope = scope;
            DefinitionSource = definitionSource;
            Segments = SegmentsParser.ParseSegments(value);
        }

        /// <summary>
        /// Gets the locator method.
        /// </summary>
        public By.ByMethod Method { get; }

        /// <summary>
        /// Gets the locator value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Gets the scope in which to search for the element.
        /// </summary>
        public By.ByScope Scope { get; }

        /// <summary>
        /// Gets the parameterized segments extracted from the locator value.
        /// </summary>
        public IList<string> Segments { get; }

        /// <summary>
        /// Gets the source file and region where this locator is defined.
        /// </summary>
        public DefinitionSource DefinitionSource { get; }
    }
}
