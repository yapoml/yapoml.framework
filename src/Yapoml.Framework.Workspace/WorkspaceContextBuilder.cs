using System.Collections.Generic;
using Yapoml.Framework.Workspace.Parsers;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Workspace
{
    public class WorkspaceContextBuilder
    {
        private readonly string _rootDirectoryPath;
        private readonly string _rootNamespace;
        private readonly IWorkspaceParser _parser;
        private IWorkspaceReferenceResolver _workspaceReferenceResolver;
        private INameNormalizer _nameNormalizer;

        private IDictionary<string, string> _files = new Dictionary<string, string>();

        public WorkspaceContextBuilder(string rootDirectoryPath, string rootNamespace, IWorkspaceParser parser)
        {
            _rootDirectoryPath = rootDirectoryPath;
            _rootNamespace = rootNamespace;
            _parser = parser;
        }

        public WorkspaceContextBuilder AddFile(string filePath, string fileContent)
        {
            _files[filePath] = fileContent;

            return this;
        }

        public WorkspaceContextBuilder WithReferenceResolver(IWorkspaceReferenceResolver workspaceReferenceResolver)
        {
            _workspaceReferenceResolver = workspaceReferenceResolver;

            return this;
        }

        public WorkspaceContextBuilder WithNameNormalizer(INameNormalizer nameNormalizer)
        {
            _nameNormalizer = nameNormalizer;

            return this;
        }

        public WorkspaceContext Build()
        {
            var workspaceContext = new WorkspaceContext(
                _rootDirectoryPath,
                _rootNamespace,
                _parser,
                _workspaceReferenceResolver ?? new WorkspaceReferenceResolver(),
                _nameNormalizer ?? new NameNormalizer());

            foreach (var file in _files)
            {
                workspaceContext.AddFile(file.Key, file.Value);
            }

            workspaceContext.ResolveReferences();

            return workspaceContext;
        }
    }
}
