using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Yapoml.Generation;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Selenium
{
    [Generator]
    public class SeleniumGeneratorGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var yamlParser = new Parsers.Yaml.YamlParser();

            foreach (AdditionalText file in context.AdditionalFiles)
            {
                if (file.Path.EndsWith(".po.yaml", StringComparison.OrdinalIgnoreCase))
                {
                    File.AppendAllText("C:\\Temp\\AddFiles.txt", file.Path);

                    var component = yamlParser.Parse<Component>(File.ReadAllText(file.Path));

                    var gContext = PageObjectGenerationContext.FromYamlComponent(file.Path, component);

                    context.AddSource("E_" + Path.GetFileNameWithoutExtension(file.Path), $@"
namespace AAA
{{
  class {gContext.ClassName}
  {{

  }}
}}
");
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    }
}

