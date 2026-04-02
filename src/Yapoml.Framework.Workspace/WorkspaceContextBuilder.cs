using System;
using System.IO;
using System.Linq;
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

        if (TryGetPageOrComponentFile(filePath, out var pageName))
        {
            var space = CreateOrAddSpaces(filePath);

            var pages = _parser.ParsePages(fileContent);

            for (int i = 0; i < pages.Count; i++)
            {
                var page = pages[i];

                // adjust page family
                if (i != 0)
                {
                    pageName = $"{pageName}_{i}";
                }

                page.Name = pageName;

                PageContext pageContext;

                if (space == null)
                {
                    pageContext = new PageContext(_workspaceContext, null, page, GetRelativeFilePath(filePath));

                    _workspaceContext.AddPage(pageContext);
                }
                else
                {
                    pageContext = new PageContext(_workspaceContext, space, page, GetRelativeFilePath(filePath));

                    space.AddPage(pageContext);
                }

                _workspaceReferenceResolver.AppendPage(pageContext);
            }
        }
        else if (TryGetComponentFile(filePath, out var componentName))
        {
            var space = CreateOrAddSpaces(filePath);

            var component = _parser.ParseComponent(fileContent);

            if (string.IsNullOrEmpty(component.Name))
            {
                component.Name = componentName;
            }

            ComponentContext componentContext;

            if (space == null)
            {
                componentContext = new ComponentContext(_workspaceContext, null, null, null, component, GetRelativeFilePath(filePath));

                _workspaceContext.AddComponent(componentContext);
            }
            else
            {
                componentContext = new ComponentContext(_workspaceContext, space, null, null, component, GetRelativeFilePath(filePath));

                space.AddComponent(componentContext);
            }

            _workspaceReferenceResolver.AppendComponent(componentContext);
        }

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

        _workspaceReferenceResolver.Resolve();

        return _workspaceContext;
    }

    private void EnsureWorkspaceCreated()
    {
        if (_workspaceContext == null)
        {
            if (_workspaceReferenceResolver == null)
            {
                _workspaceReferenceResolver = new WorkspaceReferenceResolver();
            }

            if (_nameNormalizer == null)
            {
                _nameNormalizer = new NameNormalizer();
            }

            _workspaceContext = new WorkspaceContext(
                _rootDirectoryPath,
                _rootNamespace,
                _nameNormalizer);
        }
    }

    private bool TryGetPageOrComponentFile(string filePath, out string pageName)
    {
        var fileName = Path.GetFileName(filePath);

        if (filePath.EndsWith(".page.yml", StringComparison.InvariantCultureIgnoreCase))
        {
            pageName = fileName.Substring(0, fileName.Length - ".page.yml".Length);
            return true;
        }
        else if (filePath.EndsWith(".page.yaml", StringComparison.InvariantCultureIgnoreCase))
        {
            pageName = fileName.Substring(0, fileName.Length - ".page.yaml".Length);
            return true;
        }
        else
        {
            pageName = null;
            return false;
        }
    }

    private bool TryGetComponentFile(string filePath, out string componentName)
    {
        var fileName = Path.GetFileName(filePath);

        if (filePath.EndsWith(".component.yml", StringComparison.InvariantCultureIgnoreCase))
        {
            componentName = fileName.Substring(0, fileName.Length - ".component.yml".Length);
            return true;
        }
        else if (filePath.EndsWith(".component.yaml", StringComparison.InvariantCultureIgnoreCase))
        {
            componentName = fileName.Substring(0, fileName.Length - ".component.yaml".Length);
            return true;
        }
        else
        {
            componentName = null;
            return false;
        }
    }

    private SpaceContext CreateOrAddSpaces(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);

        var path = directory.Substring(_workspaceContext.RootDirectoryPath.Length);

        var parts = path.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 0)
        {
            var normalizedSpaceName = _workspaceContext.NameNormalizer.Normalize(parts[0]);

            SpaceContext nestedSpace = _workspaceContext.Spaces.FirstOrDefault(s => s.Namespace == $"{_workspaceContext.RootNamespace}.{normalizedSpaceName}");

            if (nestedSpace == null)
            {
                nestedSpace = new SpaceContext(normalizedSpaceName, _workspaceContext, null);

                _workspaceContext.AddSpace(nestedSpace);
            }

            for (int i = 1; i < parts.Length; i++)
            {
                normalizedSpaceName = _workspaceContext.NameNormalizer.Normalize(parts[i]);

                var candidateNestedSpace = nestedSpace.Spaces.FirstOrDefault(s => s.Name == normalizedSpaceName);

                if (candidateNestedSpace == null)
                {
                    var newNestedSpace = new SpaceContext(normalizedSpaceName, _workspaceContext, nestedSpace);

                    nestedSpace.AddSpace(newNestedSpace);

                    nestedSpace = newNestedSpace;
                }
                else
                {
                    nestedSpace = candidateNestedSpace;
                }
            }

            return nestedSpace;
        }
        else
        {
            return null;
        }
    }

    private string GetRelativeFilePath(string fullFilePath)
    {
        return fullFilePath.Substring(_workspaceContext.RootDirectoryPath.Length + 1);
    }
}
