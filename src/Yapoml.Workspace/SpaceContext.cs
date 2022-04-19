using System.Collections.Generic;

namespace Yapoml.Workspace
{
    public class SpaceContext
    {
        public SpaceContext(string name, WorkspaceContext workspace, SpaceContext parentSpaceContext)
        {
            Name = NormalizeName(name);

            if (parentSpaceContext != null)
            {
                Namespace = $"{parentSpaceContext.Namespace}.{Name}";
            }
            else
            {
                Namespace = $"{workspace.RootNamespace}.{Name}";
            }

            ParentSpace = parentSpaceContext;
        }

        public string Name { get; }

        public string Namespace { get; }

        public SpaceContext ParentSpace { get; }

        public IList<SpaceContext> Spaces { get; } = new List<SpaceContext>();

        public IList<PageContext> Pages { get; } = new List<PageContext>();

        public IList<ComponentContext> Components { get; } = new List<ComponentContext>();

        private string NormalizeName(string name)
        {
            return name.Replace(" ", "_").Replace("-", "_");
        }
    }
}
