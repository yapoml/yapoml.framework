using Yapoml.Framework.Workspace.Parsers;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Workspace;

/// <summary>
/// Fluent builder for constructing a <see cref="WorkspaceContext"/> from workspace files.
/// </summary>
public class WorkspaceContextBuilder
{
    private readonly string _rootDirectoryPath;
    private readonly string _rootNamespace;
    private readonly IWorkspaceParser _parser;
    private IWorkspaceReferenceResolver _workspaceReferenceResolver;
    private INameNormalizer _nameNormalizer;

    private WorkspaceContext _workspaceContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkspaceContextBuilder"/> class.
    /// </summary>
    /// <param name="rootDirectoryPath">The root directory path of the workspace.</param>
    /// <param name="rootNamespace">The root namespace for generated code.</param>
    /// <param name="parser">The parser used to parse workspace files.</param>
    public WorkspaceContextBuilder(string rootDirectoryPath, string rootNamespace, IWorkspaceParser parser)
    {
        _rootDirectoryPath = rootDirectoryPath;
        _rootNamespace = rootNamespace;
        _parser = parser;
    }

    /// <summary>
    /// Adds a file to the workspace being built.
    /// </summary>
    /// <param name="filePath">The absolute path of the file.</param>
    /// <param name="fileContent">The content of the file.</param>
    /// <returns>This builder instance for chaining.</returns>
    public WorkspaceContextBuilder AddFile(string filePath, string fileContent)
    {
        EnsureWorkspaceCreated();

        _workspaceContext.AddFile(filePath, fileContent);

        return this;
    }

    /// <summary>
    /// Sets the reference resolver to use when building the workspace.
    /// </summary>
    /// <param name="workspaceReferenceResolver">The reference resolver.</param>
    /// <returns>This builder instance for chaining.</returns>
    public WorkspaceContextBuilder WithReferenceResolver(IWorkspaceReferenceResolver workspaceReferenceResolver)
    {
        _workspaceReferenceResolver = workspaceReferenceResolver;

        return this;
    }

    /// <summary>
    /// Sets the name normalizer to use when building the workspace.
    /// </summary>
    /// <param name="nameNormalizer">The name normalizer.</param>
    /// <returns>This builder instance for chaining.</returns>
    public WorkspaceContextBuilder WithNameNormalizer(INameNormalizer nameNormalizer)
    {
        _nameNormalizer = nameNormalizer;

        return this;
    }

    /// <summary>
    /// Builds and returns the <see cref="WorkspaceContext"/>, resolving all cross-references.
    /// </summary>
    /// <returns>The fully constructed <see cref="WorkspaceContext"/>.</returns>
    public WorkspaceContext Build()
    {
        EnsureWorkspaceCreated();

        _workspaceContext.ResolveReferences();

        return _workspaceContext;
    }

    private void EnsureWorkspaceCreated()
    {
        if (_workspaceContext == null)
        {
            _workspaceContext = new WorkspaceContext(
                _rootDirectoryPath,
                _rootNamespace,
                _parser,
                _workspaceReferenceResolver ?? new WorkspaceReferenceResolver(),
                _nameNormalizer ?? new NameNormalizer());
        }
    }
}
