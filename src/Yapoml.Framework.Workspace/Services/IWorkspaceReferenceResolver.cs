namespace Yapoml.Framework.Workspace.Services
{
    public interface IWorkspaceReferenceResolver
    {
        void AppendComponent(ComponentContext componentContext);

        void AppendPage(PageContext pageContext);

        void Resolve();
    }
}
