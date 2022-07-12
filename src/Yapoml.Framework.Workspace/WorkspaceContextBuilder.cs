using System.Collections.Generic;
using Yapoml.Framework.Workspace.Parsers;

namespace Yapoml.Framework.Workspace
{
    public class WorkspaceContextBuilder
    {
        private readonly string _rootDirectoryPath;
        private readonly string _rootNamespace;
        private readonly IWorkspaceParser _parser;

        private readonly IList<FileInfo> _files = new List<FileInfo>();

        public WorkspaceContextBuilder(string rootDirectoryPath, string rootNamespace, IWorkspaceParser parser)
        {
            _rootDirectoryPath = rootDirectoryPath;
            _rootNamespace = rootNamespace;
            _parser = parser;
        }

        public WorkspaceContextBuilder AddFile(string filePath, string fileContent)
        {
            _files.Add(new FileInfo(filePath, fileContent));

            return this;
        }

        public WorkspaceContext Build()
        {
            var globalContext = new WorkspaceContext(_rootDirectoryPath, _rootNamespace, _parser);

            foreach (var file in _files)
            {
                globalContext.AddFile(file.Path, file.Content);
            }

            globalContext.ResolveReferences();

            return globalContext;
        }

        struct FileInfo
        {
            public FileInfo(string path, string content)
            {
                Path = path;
                Content = content;
            }

            public string Path { get; }

            public string Content { get; }
        }
    }
}
