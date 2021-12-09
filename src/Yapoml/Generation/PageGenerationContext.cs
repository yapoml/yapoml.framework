using System;
using System.Collections.Generic;
using System.Text;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Generation
{
    public class PageGenerationContext
    {
        public PageGenerationContext(string name, GlobalGenerationContext globalContext, SpaceGenerationContext spaceContext, Page pageModel)
        {
            Name = name;

            if (spaceContext != null)
            {
                Namespace = $"{spaceContext.Namespace}.{Name}";
            }
            else
            {
                Namespace = $"{globalContext.RootNamespace}.{Name}";
            }

            ParentContext = spaceContext;
        }

        public string Name { get; }

        public string Namespace { get; }

        public SpaceGenerationContext ParentContext { get; }

        public IList<ComponentGenerationContext> Components { get; } = new List<ComponentGenerationContext>();
    }
}
