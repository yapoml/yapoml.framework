using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;
using Yapoml.Framework.Workspace.Parsers.Yaml;
using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers
{
    public class WorkspaceParser : IWorkspaceParser
    {
        private readonly YamlParser _yamlParser = new YamlParser();

        public IList<Page> ParsePages(string content)
        {
            var models = _yamlParser.ParseMany<Page>(content);

            return models;
        }

        public Component ParseComponent(string content)
        {
            var model = _yamlParser.Parse<Component>(content);

            return model;
        }
    }
}
