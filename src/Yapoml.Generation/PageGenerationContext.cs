using System;
using System.Collections.Generic;
using System.Text;
using Yapoml.Generation.Parsers.Yaml.Pocos;

namespace Yapoml.Generation
{
    public class PageGenerationContext
    {
        public PageGenerationContext(string name, GlobalGenerationContext globalContext, SpaceGenerationContext spaceContext, Page pageModel)
        {
            Name = name.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            if (spaceContext != null)
            {
                Namespace = $"{spaceContext.Namespace}";
            }
            else
            {
                Namespace = $"{globalContext.RootNamespace}";
            }

            ParentContext = spaceContext;

            if (pageModel.Components != null)
            {
                foreach (var component in pageModel.Components)
                {
                    Components.Add(new ComponentGenerationContext(component.Key, globalContext, spaceContext, component.Value));
                }
            }
        }

        public string Name { get; }

        public string Namespace { get; }

        public SpaceGenerationContext ParentContext { get; }

        public IList<ComponentGenerationContext> Components { get; } = new List<ComponentGenerationContext>();
    }
}
