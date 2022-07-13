using Yapoml.Framework.Workspace.Parsers;

namespace Yapoml.Framework.Workspace
{
    public class WorkspaceContextBuilder
    {
        WorkspaceContext workspaceContext;

        public WorkspaceContextBuilder(string rootDirectoryPath, string rootNamespace, IWorkspaceParser parser)
        {
            workspaceContext = new WorkspaceContext(rootDirectoryPath, rootNamespace, parser);
        }

        public WorkspaceContextBuilder AddFile(string filePath, string fileContent)
        {
            workspaceContext.AddFile(filePath, fileContent);

            return this;
        }

        public WorkspaceContext Build()
        {
            workspaceContext.ResolveReferences();

            return workspaceContext;
        }
    }
}
