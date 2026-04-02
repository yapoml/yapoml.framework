namespace Yapoml.Framework.Workspace.Services;

/// <summary>
/// Defines the contract for resolving cross-references between pages and components in a workspace.
/// </summary>
public interface IWorkspaceReferenceResolver
{
    /// <summary>
    /// Registers a component to be resolved.
    /// </summary>
    /// <param name="componentContext">The component context to register.</param>
    void AppendComponent(ComponentContext componentContext);

    /// <summary>
    /// Registers a page to be resolved.
    /// </summary>
    /// <param name="pageContext">The page context to register.</param>
    void AppendPage(PageContext pageContext);

    /// <summary>
    /// Resolves all registered cross-references between pages and components.
    /// </summary>
    /// <exception cref="ReferenceResolutionException">Thrown when a reference cannot be resolved.</exception>
    void Resolve();
}
