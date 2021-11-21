using System;
using System.Collections.Generic;
using System.Text;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Generation
{
    public class ComponentGenerationContext
    {
        public ComponentGenerationContext(GlobalGenerationContext globalGenerationContext, SpaceGenerationContext spaceGenerationContext, Component component)
        {
            GlobalGenerationContext = globalGenerationContext;
            SpaceGenerationContext = spaceGenerationContext;
            Component = component;
        }

        public GlobalGenerationContext GlobalGenerationContext { get; }
        public SpaceGenerationContext SpaceGenerationContext { get; }
        public Component Component { get; }
    }
}
