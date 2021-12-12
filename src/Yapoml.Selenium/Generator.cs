using System;
using System.IO;
using DotLiquid;
using Microsoft.CodeAnalysis;
using Yapoml.Generation;
using Yapoml.Parsers;

namespace Yapoml.Selenium
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        private GeneratorExecutionContext _context;

        private TemplateReader _templateReader;

        private string _rootNamespace;

        public void Execute(GeneratorExecutionContext context)
        {
            _context = context;
            _templateReader = new TemplateReader();

            try
            {
                Template.RegisterSafeType(typeof(SpaceGenerationContext), new string[]
                {
                    nameof(SpaceGenerationContext.Name),
                    nameof(SpaceGenerationContext.Namespace),
                    nameof(SpaceGenerationContext.Pages)
                });

                Template.RegisterSafeType(typeof(PageGenerationContext), new string[]
                {
                    nameof(PageGenerationContext.Name),
                    nameof(PageGenerationContext.Namespace)
                });

                // get root namespace
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.RootNamespace", out _rootNamespace);
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ProjectDir", out var projectDir);

                var parser = new Parser();

                // build yapoml generation context
                var yaContext = new GlobalGenerationContext(projectDir, _rootNamespace, parser);

                foreach (AdditionalText file in context.AdditionalFiles)
                {
                    yaContext.AddFile(file.Path);
                }

                // generate files
                GenerateEntryPoint(yaContext);

                foreach (var space in yaContext.Spaces)
                {
                    GenerateSpaces(space);
                }

                foreach (var page in yaContext.Pages)
                {
                    GeneratePages(page);
                }
            }
            catch (Exception exp)
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

        public void GenerateEntryPoint(GlobalGenerationContext globalGenerationContext)
        {
            Template engine = Template.Parse(_templateReader.Read("EntryPointTemplate"));

            var renderedEntryPoint = engine.Render(Hash.FromAnonymousObject(globalGenerationContext));

            _context.AddSource("_EntryPoint.cs", renderedEntryPoint);
        }

        public void GenerateSpaces(SpaceGenerationContext spaceGenerationContext)
        {
            Template engine = Template.Parse(_templateReader.Read("SpaceTemplate"));

            var renderedSpace = engine.Render(Hash.FromAnonymousObject(spaceGenerationContext));

            var generatedFileName = $"{spaceGenerationContext.Namespace.Substring(_rootNamespace.Length + 1).Replace('.', '_')}_{spaceGenerationContext.Name}Space.cs";
            _context.AddSource(generatedFileName, renderedSpace);

            foreach (var space in spaceGenerationContext.Spaces)
            {
                GenerateSpaces(space);
            }

            foreach (var page in spaceGenerationContext.Pages)
            {
                GeneratePages(page);
            }

            foreach (var component in spaceGenerationContext.Components)
            {
                GenerateComponent(component);
            }
        }

        public void GeneratePages(PageGenerationContext pageGenerationContext)
        {
            Template engine = Template.Parse(_templateReader.Read("PageTemplate"));

            var renderedPage = engine.Render(Hash.FromAnonymousObject(pageGenerationContext));

            var generatedFileName = $"{pageGenerationContext.Namespace.Substring(_rootNamespace.Length + 1).Replace('.', '_')}_{pageGenerationContext.Name}Page.g.cs";
            _context.AddSource(generatedFileName, renderedPage);

            foreach (var component in pageGenerationContext.Components)
            {
                GenerateComponent(component);
            }
        }

        public void GenerateComponent(ComponentGenerationContext componentGenerationContext)
        {
            Template engine = Template.Parse(_templateReader.Read("ComponentTemplate"));

            var renderedComponent = engine.Render(Hash.FromAnonymousObject(componentGenerationContext));

            var generatedFileName = $"{componentGenerationContext.Namespace.Substring(_rootNamespace.Length + 1).Replace('.', '_')}_{componentGenerationContext.Name}Component.g.cs";
            _context.AddSource(generatedFileName, renderedComponent);

            foreach (var component in componentGenerationContext.ComponentGenerationContextes)
            {
                GenerateComponent(component);
            }
        }
    }
}

