using System.Collections.Generic;
using Yapoml.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Workspace
{
    public class ComponentContext
    {
        public ComponentContext(string name, WorkspaceContext workspace, SpaceContext space, Parsers.Yaml.Pocos.Component component)
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

            By = component.By;

            if (space != null)
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
                    Components.Add(new ComponentContext(nestedComponent.Name, workspace, space, nestedComponent));
                }
            }
        }

        public WorkspaceContext Workspace { get; }

        public SpaceContext Space { get; }

        public string Name { get; }

        public string Namespace { get; }

        public By By { get; }

        public bool IsPlural
        {
            get
            {
                var pluralityService = new Services.PluralizationService();

                return pluralityService.IsPlural(Name);
            }
        }

        public string SingularName
        {
            get
            {
                var pluralityService = new Services.PluralizationService();

                return pluralityService.Singularize(Name);
            }
        }

        public IList<ComponentContext> Components { get; } = new List<ComponentContext>();
    }
}
