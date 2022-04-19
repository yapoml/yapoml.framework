using System.IO;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;
using Yapoml.Framework.Workspace.Parsers.Yaml;

namespace Yapoml.Framework.Workspace.Parsers
{
    public class WorkspaceParser : IWorkspaceParser
    {
        public Page ParsePage(string filePath)
        {
            var content = File.ReadAllText(filePath);

            var model = new YamlParser().Parse<Page>(content);

            return model;
        }

        public Component ParseComponent(string filePath)
        {
            var content = File.ReadAllText(filePath);

            var model = new YamlParser().Parse<Component>(content);

            return model;
        }
    }
}
