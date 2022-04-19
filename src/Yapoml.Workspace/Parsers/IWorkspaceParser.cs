using Yapoml.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Workspace.Parsers
{
    public interface IWorkspaceParser
    {
        Yaml.Pocos.Page ParsePage(string filePath);

        Yaml.Pocos.Component ParseComponent(string filePath);
    }
}
