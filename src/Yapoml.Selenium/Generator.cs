using System;
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
                    nameof(SpaceGenerationContext.Namespace)
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

        public void GenerateSpaces(SpaceGenerationContext spaceGenerationContext)
        {
            Template engine = Template.Parse(_templateReader.Read("SpaceTemplate"));

            var renderedSpace = engine.Render(Hash.FromAnonymousObject(spaceGenerationContext));

            var generatedFileName = $"{spaceGenerationContext.Namespace.Substring(_rootNamespace.Length + 1).Replace('.', '_')}_{spaceGenerationContext.Name}Space.g.cs";
            _context.AddSource(generatedFileName, renderedSpace);

            foreach (var space in spaceGenerationContext.Spaces)
            {
                GenerateSpaces(space);
            }
        }

        public void GenerateEntryPoint(GlobalGenerationContext globalGenerationContext)
        {
            Template engine = Template.Parse(_templateReader.Read("EntryPointTemplate"));

            var renderedEntryPoint = engine.Render(Hash.FromAnonymousObject(globalGenerationContext));

            _context.AddSource("_EntryPoint.ggg.cs", renderedEntryPoint);
        }
    }
}

