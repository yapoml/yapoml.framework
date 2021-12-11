using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ProjectDir", out var projectDir);

                var parser = new Parser();

                // build yapoml generation context
                var yaContext = new GlobalGenerationContext(projectDir, rootNamespace, parser);

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

            _context.AddSource($"{Path.GetFileNameWithoutExtension(spaceGenerationContext.Name)}.ggg.cs", renderedSpace);

            foreach (var space in spaceGenerationContext.Spaces)
            {
                GenerateSpaces(space);
            }
        }

        public void GenerateEntryPoint(GlobalGenerationContext globalGenerationContext)
        {
            Template engine = Template.Parse(_templateReader.Read("EntryPointTemplate"));

            var renderedEntryPoint = engine.Render(Hash.FromAnonymousObject(globalGenerationContext));

            _context.AddSource("EntryPoint.ggg.cs", renderedEntryPoint);
        }
    }
}

