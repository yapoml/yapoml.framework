using System.Collections.Generic;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Workspace
{
    public class ComponentContext
    {
        public ComponentContext(string name, WorkspaceContext workspace, SpaceContext space, PageContext page, ComponentContext parentComponent, Component component)
        {
            Workspace = workspace;
            Space = space;

            if (!string.IsNullOrEmpty(component.Name))
            {
                Name = component.Name;
            }
            else
            {
                Name = name;
            }

            if (component.By != null)
            {
                By = new ByContext(component.By.Method, component.By.Value);
            }

            if (parentComponent != null)
            {
                Namespace = $"{parentComponent.Namespace}.{parentComponent.SingularName}Component";
            }
            else if (page != null)
            {
                Namespace = $"{page.Namespace}.{page.Name}";
            }
            else if (space != null)
            {
                Namespace = space.Namespace;
            }
            else
            {
                Namespace = workspace.RootNamespace;
            }

            if (component.Components != null)
            {
                foreach (var nestedComponent in component.Components)
                {
                    Components.Add(new ComponentContext(nestedComponent.Name, workspace, space, null, this, nestedComponent));
                }
            }

            if (component.Ref != null)
            {
                ReferencedComponentName = component.Ref;
            }
        }

        public WorkspaceContext Workspace { get; }

        public SpaceContext Space { get; }

        public string Name { get; }

        public string Namespace { get; }

        public ByContext By { get; set; }

        public string ReferencedComponentName { get; }

        public ComponentContext ReferencedComponent { get; set; }

        private bool? _isPlural;

        public bool IsPlural
        {
            get
            {

                if (!_isPlural.HasValue)
                {
                    _isPlural = new Services.PluralizationService().IsPlural(Name);
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
                    _singularName = new Services.PluralizationService().Singularize(Name);
                }

                return _singularName;
            }
        }

        public IList<ComponentContext> Components { get; } = new List<ComponentContext>();

        public class ByContext
        {
            public ByContext(By.ByMethod method, string value)
            {
                Method = method;
                Value = value;
                Segments = SegmentsParser.ParseSegments(value);
            }

            public By.ByMethod Method { get; }
            public string Value { get; }
            public IList<string> Segments { get; }
        }
    }
}
