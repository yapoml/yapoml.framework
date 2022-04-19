using System.IO;
using Yapoml.Workspace.Parsers.Yaml;
using Yapoml.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Workspace.Parsers
{
    public class WorkspaceParser : IWorkspaceParser
    {
        public Yaml.Pocos.Page ParsePage(string filePath)
        {
            var content = File.ReadAllText(filePath);

            var model = new YamlParser().Parse<Yaml.Pocos.Page>(content);

            return model;
        }

        public Yaml.Pocos.Component ParseComponent(string filePath)
        {
            var content = File.ReadAllText(filePath);

            var model = new YamlParser().Parse<Yaml.Pocos.Component>(content);

            return model;
        }
    }
}
