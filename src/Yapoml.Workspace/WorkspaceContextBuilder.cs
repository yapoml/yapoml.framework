using System.Collections.Generic;
using Yapoml.Workspace.Parsers;

namespace Yapoml.Workspace
{
    public class WorkspaceContextBuilder
    {
        private readonly string _rootDirectoryPath;
        private readonly string _rootNamespace;
        private readonly IWorkspaceParser _parser;

        private readonly IList<string> _files = new List<string>();

        public WorkspaceContextBuilder(string rootDirectoryPath, string rootNamespace, IWorkspaceParser parser)
        {
            _rootDirectoryPath = rootDirectoryPath;
            _rootNamespace = rootNamespace;
            _parser = parser;
        }

        public WorkspaceContextBuilder AddFile(string filePath)
        {
            _files.Add(filePath);

            return this;
        }

        public WorkspaceContext Build()
        {
            var globalContext = new WorkspaceContext(_rootDirectoryPath, _rootNamespace, _parser);

            foreach (var filePath in _files)
            {
                globalContext.AddFile(filePath);
            }

            globalContext.ResolveReferences();

            return globalContext;
        }
    }
}
