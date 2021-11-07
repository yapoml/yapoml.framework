using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Generation
{
    public class PageObjectGenerationContext
    {
        public PageObjectGenerationContext(string filePath)
        {
            ClassName = GetClassName(filePath);
        }

        private string GetClassName(string filePath)
        {
            var fileName = Path.GetFileName(filePath).Replace(".po.yaml", string.Empty);

            // replace denied characters
            var deniedCharacters = new string[] { " ", "-" };
            foreach (var deniedCharacter in deniedCharacters)
            {
                fileName = fileName.Replace(deniedCharacter, string.Empty);
            }

            return fileName;
        }

        public string ClassName { get; }

        public static PageObjectGenerationContext FromYamlComponent(string filePath, Component component)
        {
            return new PageObjectGenerationContext(filePath);
        }
    }
}
