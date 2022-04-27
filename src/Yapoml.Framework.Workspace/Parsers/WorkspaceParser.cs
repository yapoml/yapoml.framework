using System.IO;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;
using Yapoml.Framework.Workspace.Parsers.Yaml;
using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers
{
    public class WorkspaceParser : IWorkspaceParser
    {
        private readonly YamlParser _yamlParser = new YamlParser();

        public IList<Page> ParsePages(string filePath)
        {
            var content = File.ReadAllText(filePath);

            var models = _yamlParser.ParseMany<Page>(content);

            return models;
        }

        public Component ParseComponent(string filePath)
        {
            var content = File.ReadAllText(filePath);

            var model = _yamlParser.Parse<Component>(content);

            return model;
        }
    }
}
