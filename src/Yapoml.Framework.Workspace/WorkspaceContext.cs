using System.Collections.Generic;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Workspace;

/// <summary>
/// Represents the root context of a Yapoml workspace, containing all discovered spaces, pages, and components.
/// </summary>
public class WorkspaceContext
{
    private readonly List<SpaceContext> _spaces = new List<SpaceContext>();
    private readonly List<PageContext> _pages = new List<PageContext>();
    private readonly List<ComponentContext> _components = new List<ComponentContext>();

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkspaceContext"/> class.
    /// </summary>
    /// <param name="rootDirectoryPath">The root directory path of the workspace.</param>
    /// <param name="rootNamespace">The root namespace used for generated code.</param>
    /// <param name="nameNormalizer">The normalizer used to convert raw names to valid identifiers.</param>
    internal WorkspaceContext(string rootDirectoryPath, string rootNamespace, INameNormalizer nameNormalizer)
    {
        RootDirectoryPath = rootDirectoryPath.Replace("/", "\\").TrimEnd('\\');
        RootNamespace = rootNamespace;
        NameNormalizer = nameNormalizer;
    }

    /// <summary>
    /// Gets the root directory path of the workspace.
    /// </summary>
    public string RootDirectoryPath { get; }

    /// <summary>
    /// Gets the root namespace used for generated code.
    /// </summary>
    public string RootNamespace { get; }

    /// <summary>
    /// Gets the name normalizer used to convert raw names to valid identifiers.
    /// </summary>
    public INameNormalizer NameNormalizer { get; }

    /// <summary>
    /// Gets the list of top-level spaces in the workspace.
    /// </summary>
    public IReadOnlyList<SpaceContext> Spaces => _spaces;

    /// <summary>
    /// Gets the list of top-level pages in the workspace.
    /// </summary>
    public IReadOnlyList<PageContext> Pages => _pages;

    /// <summary>
    /// Gets the list of top-level components in the workspace.
    /// </summary>
    public IReadOnlyList<ComponentContext> Components => _components;

    /// <summary>
    /// Gets the version of the framework assembly.
    /// </summary>
    public string Version
    {
        get
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString();
        }
    }

    internal void AddSpace(SpaceContext space) => _spaces.Add(space);

    internal void AddPage(PageContext page) => _pages.Add(page);

    internal void AddComponent(ComponentContext component) => _components.Add(component);
}
