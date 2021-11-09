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
            // get root namespace
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);

            var yamlParser = new Parsers.Yaml.YamlParser();

            foreach (AdditionalText file in context.AdditionalFiles)
            {
                if (file.Path.EndsWith(".po.yaml", StringComparison.OrdinalIgnoreCase))
                {
                    File.AppendAllText("C:\\Temp\\AddFiles.txt", file.Path);

                    var component = yamlParser.Parse<Component>(File.ReadAllText(file.Path));

                    var gContext = PageObjectGenerationContext.FromYamlComponent(file.Path, component);

                    context.AddSource("E_" + Path.GetFileNameWithoutExtension(file.Path), $@"
namespace {rootNamespace}
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

