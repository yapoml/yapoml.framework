using System.Collections.Generic;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Generation
{
    public class ComponentGenerationContext
    {
        public ComponentGenerationContext(GlobalGenerationContext globalGenerationContext, SpaceGenerationContext spaceGenerationContext, Component component)
        {
            GlobalGenerationContext = globalGenerationContext;
            SpaceGenerationContext = spaceGenerationContext;

            Name = component.Name;
            By = component.By;

            if (component.Components != null)
            {
                foreach (var nestedComponent in component.Components)
                {
                    ComponentGenerationContextes.Add(new ComponentGenerationContext(globalGenerationContext, spaceGenerationContext, nestedComponent.Value));
                }
            }
        }

        public GlobalGenerationContext GlobalGenerationContext { get; }

        public SpaceGenerationContext SpaceGenerationContext { get; }

        public string Name { get; }

        public By By { get; }

        public bool IsPlural
        {
            get
            {
                throw new System.Exception("not yet implemented");
            }
        }

        public IList<ComponentGenerationContext> ComponentGenerationContextes { get; } = new List<ComponentGenerationContext>();
    }
}
