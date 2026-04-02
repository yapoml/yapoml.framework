using System.Collections.Generic;

namespace Yapoml.Framework.Workspace;

/// <summary>
/// Represents a namespace-like grouping of pages and components within the workspace hierarchy.
/// </summary>
public class SpaceContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpaceContext"/> class.
    /// </summary>
    /// <param name="name">The name of the space.</param>
    /// <param name="workspace">The workspace this space belongs to.</param>
    /// <param name="parentSpaceContext">The parent space, or <see langword="null"/> if this is a top-level space.</param>
    public SpaceContext(string name, WorkspaceContext workspace, SpaceContext parentSpaceContext)
    {
        Workspace = workspace;
        ParentSpace = parentSpaceContext;

        Name = name;
    }

    /// <summary>
    /// Gets the name of the space.
    /// </summary>
    public string Name { get; }

    private string _namespace;

    /// <summary>
    /// Gets the fully qualified namespace of this space.
    /// </summary>
    public string Namespace
    {
        get
        {
            if (_namespace is null)
            {
                if (ParentSpace != null)
                {
                    _namespace = $"{ParentSpace.Namespace}.{Name}";
                }
                else
                {
                    _namespace = $"{Workspace.RootNamespace}.{Name}";
                }
            }

            return _namespace;
        }
    }

    /// <summary>
    /// Gets the workspace this space belongs to.
    /// </summary>
    public WorkspaceContext Workspace { get; }

    /// <summary>
    /// Gets the parent space, or <see langword="null"/> if this is a top-level space.
    /// </summary>
    public SpaceContext ParentSpace { get; }

    /// <summary>
    /// Gets the child spaces within this space.
    /// </summary>
    public IList<SpaceContext> Spaces { get; } = new List<SpaceContext>();

    /// <summary>
    /// Gets the pages within this space.
    /// </summary>
    public IList<PageContext> Pages { get; } = new List<PageContext>();

    /// <summary>
    /// Gets the components within this space.
    /// </summary>
    public IList<ComponentContext> Components { get; } = new List<ComponentContext>();
}
