using System.Collections.Generic;
using System.Linq;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Workspace
{
    public class ComponentContext
    {
        private readonly Component _component;

        public ComponentContext(WorkspaceContext workspace, SpaceContext space, PageContext page, ComponentContext parentComponent, Component component, string relativeFilePath = null)
        {
            Workspace = workspace;
            Space = space;
            Page = page;
            ParentComponent = parentComponent;

            _component = component;
            RelativeFilePath = relativeFilePath;
        }

        public WorkspaceContext Workspace { get; }

        public SpaceContext Space { get; }

        public PageContext Page { get; }

        public ComponentContext ParentComponent { get; }

        private string _relativeFilePath;
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
            set
            {
                _relativeFilePath = value;
            }
        }

        private string _name;
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
        public string Namespace
        {
            get
            {
                if (_namespace is null)
                {
                    if (ParentComponent != null)
                    {
                        if (ParentComponent.SingularName.EndsWith("Component"))
                        {
                            _namespace = $"{ParentComponent.Namespace}.{ParentComponent.SingularName}";
                        }
                        else
                        {
                            _namespace = $"{ParentComponent.Namespace}.{ParentComponent.SingularName}Component";
                        }
                    }
                    else if (Page != null)
                    {
                        if (Page.Name.EndsWith("Page"))
                        {
                            _namespace = $"{Page.Namespace}.{Page.Name}";
                        }
                        else
                        {
                            _namespace = $"{Page.Namespace}.{Page.Name}Page";
                        }
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
            set
            {
                _by = value;
            }
        }

        private string _baseComponentName;
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

        public ComponentContext BaseComponent { get; set; }

        private bool? _isPlural;

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

        public class ByContext
        {
            public ByContext(By.ByMethod method, string value, By.ByScope scope, DefinitionSource definitionSource)
            {
                Method = method;
                Value = value;
                Scope = scope;
                DefinitionSource = definitionSource;
                Segments = SegmentsParser.ParseSegments(value);
            }

            public By.ByMethod Method { get; }

            public string Value { get; }

            public By.ByScope Scope { get; }

            public IList<string> Segments { get; }

            public DefinitionSource DefinitionSource { get; }
        }
    }
}
