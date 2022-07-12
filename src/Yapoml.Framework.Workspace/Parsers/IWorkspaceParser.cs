using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers
{
    public interface IWorkspaceParser
    {
        IList<Yaml.Pocos.Page> ParsePages(string content);

        Yaml.Pocos.Component ParseComponent(string content);
    }
}
