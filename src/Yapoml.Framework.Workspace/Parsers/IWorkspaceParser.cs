using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers
{
    public interface IWorkspaceParser
    {
        IList<Yaml.Pocos.Page> ParsePages(string filePath);

        Yaml.Pocos.Component ParseComponent(string filePath);
    }
}
