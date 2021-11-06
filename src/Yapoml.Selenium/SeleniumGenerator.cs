using System;
using System.IO;
using Microsoft.CodeAnalysis;

namespace Yapoml.Selenium
{
    [Generator]
    public class SeleniumGeneratorGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            foreach (AdditionalText file in context.AdditionalFiles)
            {
                if (file.Path.EndsWith(".po.yaml", StringComparison.OrdinalIgnoreCase))
                {
                    context.AddSource("E_" + Path.GetFileNameWithoutExtension(file.Path), $@"
namespace AAA
{{
  class {Path.GetFileNameWithoutExtension(file.Path).Replace(" ", "").Replace("-", "").Replace(".", "_")}
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

