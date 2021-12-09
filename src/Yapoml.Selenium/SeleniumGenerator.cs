using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            try
            {
                // get root namespace
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ProjectDir", out var projectDir);

                var pageObjectsEntryFullClassNames = new List<(string fullClassName, string className)>();

                var yamlParser = new Parsers.Yaml.YamlParser();

                foreach (AdditionalText file in context.AdditionalFiles)
                {
                    if (file.Path.EndsWith(".po.yaml", StringComparison.OrdinalIgnoreCase))
                    {
                        var component = yamlParser.Parse<Component>(file.GetText().ToString());

                        var gContext = OldPageObjectGenerationContext.FromYamlComponent(file.Path, component);

                        var builder = new StringBuilder();
                        builder.Append($@"
namespace {rootNamespace}
{{
  public class {gContext.ClassName}
  {{
        OpenQA.Selenium.IWebDriver _driver;

        public {gContext.ClassName}(OpenQA.Selenium.IWebDriver driver) {{_driver = driver;}}

");

                        if (component.Components != null)
                        {
                            foreach (var item in component.Components)
                            {
                                GenerateComponent(builder, item.Value, item.Key, $"{rootNamespace}.BaseComponent", "_driver");
                            }
                        }

                        builder.Append($@"
  }}
}}
");

                        context.AddSource(Path.GetFileNameWithoutExtension(file.Path), builder.ToString());

                        pageObjectsEntryFullClassNames.Add(($"{rootNamespace}.{gContext.ClassName}", gContext.ClassName));
                    }
                }

                GenerateBaseComponent(context, rootNamespace);

                GenerateYapomlEntry(context, pageObjectsEntryFullClassNames);
            }
            catch(Exception exp)
            {
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                    "YA0001",
                    exp.Message,
                    exp.ToString(),
                    "some category",
                    DiagnosticSeverity.Error,
                    true), null));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }

        private void GenerateComponent(StringBuilder builder, Component component, string name, string baseComponentPath, string searchContext = "_element")
        {
            /*
            public SomeComponent C1 => new SomeComponent(_driver, );
            
            class SomeComponent {
            
            }
            */

            var isPlural = name.EndsWith("s");
            if (!isPlural)
            {
                string findCode;
                if (component.By.Method == By.ByMethod.XPath)
                {
                    findCode = $@"{searchContext}.FindElement(OpenQA.Selenium.By.XPath(""{component.By.Value}""))";
                }
                else if (component.By.Method == By.ByMethod.Css)
                {
                    findCode = $@"{searchContext}.FindElement(OpenQA.Selenium.By.CssSelector(""{component.By.Value}""))";
                }
                else if (component.By.Method == By.ByMethod.Id)
                {
                    findCode = $@"{searchContext}.FindElement(OpenQA.Selenium.By.Id(""{component.By.Value}""))";
                }
                else
                {
                    throw new Exception("Unknown by method in po yaml");
                }

                builder.AppendLine($"public {name}Component {name} => new {name}Component(_driver, {findCode});");
            }
            else
            {
                string findCode;
                if (component.By.Method == By.ByMethod.XPath)
                {
                    findCode = $@"{searchContext}.FindElements(OpenQA.Selenium.By.XPath(""{component.By.Value}""))";
                }
                else if (component.By.Method == By.ByMethod.Css)
                {
                    findCode = $@"{searchContext}.FindElements(OpenQA.Selenium.By.CssSelector(""{component.By.Value}""))";
                }
                else if (component.By.Method == By.ByMethod.Id)
                {
                    findCode = $@"{searchContext}.FindElements(OpenQA.Selenium.By.Id(""{component.By.Value}""))";
                }
                else
                {
                    throw new Exception("Unknown by method in po yaml");
                }

                builder.AppendLine($"public System.Collections.Generic.IList<{name}Component> {name} {{ get {{");

                builder.AppendLine($"var components = new System.Collections.Generic.List<{name}Component>();");
                builder.AppendLine($"var elements = {findCode};");
                builder.AppendLine($"foreach (var element in elements) {{");
                builder.AppendLine($"components.Add(new {name}Component(_driver, element));");
                builder.AppendLine("}");

                builder.AppendLine("return components;");

                builder.AppendLine("} }");
            }


            builder.AppendLine($"public class {name}Component : {baseComponentPath} {{");
            builder.AppendLine($"private OpenQA.Selenium.IWebDriver _driver;");
            builder.AppendLine($"private OpenQA.Selenium.IWebElement _element;");
            builder.AppendLine($"public {name}Component(OpenQA.Selenium.IWebDriver driver, OpenQA.Selenium.IWebElement element) : base(element) {{_driver = driver; _element = element;}}");

            if (component.Components != null)
            {
                foreach (var item in component.Components)
                {
                    GenerateComponent(builder, item.Value, item.Key, baseComponentPath);
                }
            }

            builder.AppendLine("}");
        }

        private void GenerateBaseComponent(GeneratorExecutionContext context, string rootNamespace)
        {
            var classContent = $@"
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Drawing;

namespace {rootNamespace}
{{
    public class BaseComponent : IWebElement
    {{
        private readonly IWebElement _element;

        public BaseComponent(IWebElement element)
        {{
            _element = element;
        }}
        public string TagName => _element.TagName;

        public string Text => _element.Text;

        public bool Enabled => _element.Enabled;

        public bool Selected => _element.Selected;

        public Point Location => _element.Location;

        public Size Size => _element.Size;

        public bool Displayed => _element.Displayed;

        public void Clear()
        {{
            _element.Clear();
        }}

        public void Click()
        {{
            _element.Click();
        }}

        public IWebElement FindElement(By by)
        {{
            return _element.FindElement(by);
        }}

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {{
            return _element.FindElements(by);
        }}

        public string GetAttribute(string attributeName)
        {{
            return _element.GetAttribute(attributeName);
        }}

        public string GetCssValue(string propertyName)
        {{
            return _element.GetCssValue(propertyName);
        }}

        public string GetDomAttribute(string attributeName)
        {{
            return _element.GetDomAttribute(attributeName);
        }}

        public string GetDomProperty(string propertyName)
        {{
            return _element.GetDomProperty(propertyName);
        }}

        public string GetProperty(string propertyName)
        {{
            return _element.GetProperty(propertyName);
        }}

        public ISearchContext GetShadowRoot()
        {{
            return _element.GetShadowRoot();
        }}

        public void SendKeys(string text)
        {{
            _element.SendKeys(text);
        }}

        public void Submit()
        {{
            _element.Submit();
        }}
    }}
}}
";

            context.AddSource("Yapoml.BaseComponent.cs", classContent);
        }

        private void GenerateYapomlEntry(GeneratorExecutionContext context, List<(string fullClassName, string className)> pages)
        {
            var builder = new StringBuilder();
            builder.AppendLine("using OpenQA.Selenium;");
            builder.AppendLine("namespace Yapoml {");

            builder.AppendLine("  public static class YaExtensions {");

            builder.AppendLine("    public static PageObjectRepository Ya(this IWebDriver driver) {");
            builder.AppendLine("      return new PageObjectRepository(driver);");
            builder.AppendLine("    }");

            builder.AppendLine("    public class PageObjectRepository {");
            builder.AppendLine("      IWebDriver _driver;");
            builder.AppendLine("      public PageObjectRepository(IWebDriver driver) {_driver = driver;}");

            foreach (var page in pages)
            {
                builder.AppendLine($"public {page.fullClassName} {page.className} => new {page.fullClassName}(_driver);");
            }

            builder.AppendLine("    }");

            builder.AppendLine("  }");

            builder.AppendLine("}");

            context.AddSource("Yapoml.cs", builder.ToString());
        }
    }
}

