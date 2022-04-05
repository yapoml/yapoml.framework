using System.Collections.Generic;

namespace Yapoml.Generation
{
    public class SpaceGenerationContext
    {
        public SpaceGenerationContext(string name, GlobalGenerationContext globalContext, SpaceGenerationContext parentContext)
        {
            Name = NormalizeName(name);

            if (parentContext != null)
            {
                Namespace = $"{parentContext.Namespace}.{Name}";
            }
            else
            {
                Namespace = $"{globalContext.RootNamespace}.{Name}";
            }

            ParentContext = parentContext;
        }

        public string Name { get; }

        public string Namespace { get; }

        public SpaceGenerationContext ParentContext { get; }

        public IList<SpaceGenerationContext> Spaces { get; } = new List<SpaceGenerationContext>();

        public IList<PageGenerationContext> Pages { get; } = new List<PageGenerationContext>();

        public IList<ComponentGenerationContext> Components { get; } = new List<ComponentGenerationContext>();

        private string NormalizeName(string name)
        {
            return name.Replace(" ", "_").Replace("-", "_");
        }
    }
}
