using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using Yapoml.Framework.Workspace.Parsers;
using Yapoml.Framework.Workspace;
using System.Linq;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Test.Workspace
{
    internal class ComponentTests
    {
        private WorkspaceParser _parser = new WorkspaceParser();
        private INameNormalizer _nameNormalizer = new NameNormalizer();

        [Test]
        public void Parse_Component()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.component.yaml"), @"
by: qwe

c2:
  by: asd
");

            gc.Spaces.Should().BeEmpty();

            gc.Components.Should().HaveCount(1);

            var component = gc.Components[0];
            component.Name.Should().Be("MyComponent");
            component.RelativeFilePath.Should().Be("my_component.component.yaml");

            component.Components[0].Name.Should().Be("C2");
            component.Components[0].RelativeFilePath.Should().Be("my_component.component.yaml");
        }

        [Test]
        public void Component_Name_Should_Be_Optional()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.component.yml"), @"
by: qwe
");

            gc.Spaces.Should().BeEmpty();

            gc.Components.Should().HaveCount(1);

            var component = gc.Components[0];
            component.Name.Should().Be("MyComponent");
        }

        [Test]
        public void Component_Segment()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.component.yaml"), @"
by: qwe {param1}
");

            var component = gc.Components[0];
            component.By.Should().NotBeNull();
            component.By.Value.Should().Be("qwe {param1}");
            component.By.Segments.Should().HaveCount(1);
            component.By.Segments.First().Should().Be("param1");
        }

        [Test]
        public void Component_DefinitionSource()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "qwe/my_component.component.yaml"), "by: 12345");

            var component = gc.Spaces[0].Components[0];
            var byDefinitionSource = component.By.DefinitionSource;
            byDefinitionSource.RelativeFilePath.Should().Be("qwe/my_component.component.yaml");
            byDefinitionSource.Region.Start.Should().Be(new Region.Position(1, 5));
            byDefinitionSource.Region.End.Should().Be(new Region.Position(1, 9));
        }

        [Test]
        public void Component_Inherited_ByDefinitionSource()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "qwe/my.component.yaml"), "by: 12345");

            var component = gc.Spaces[0].Components[0];
            var byDefinitionSource = component.By.DefinitionSource;
            byDefinitionSource.RelativeFilePath.Should().Be("qwe/my.component.yaml");
            byDefinitionSource.Region.Start.Should().Be(new Region.Position(1, 5));
            byDefinitionSource.Region.End.Should().Be(new Region.Position(1, 9));
        }

        [Test]
        public void Component_Inherited_ByDefinitionSource_IfNotSpecified()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "qwe/my_base.component.yaml"), "by: 12345");
            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "qwe/my.component.yaml"), "base: my_base");

            gc.ResolveReferences();

            var component = gc.Spaces[0].Components[1];
            var byDefinitionSource = component.By.DefinitionSource;
            byDefinitionSource.RelativeFilePath.Should().Be("qwe/my_base.component.yaml");
            byDefinitionSource.Region.Start.Should().Be(new Region.Position(1, 5));
            byDefinitionSource.Region.End.Should().Be(new Region.Position(1, 9));
        }

        [Test]
        public void Component_Segments()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.component.yaml"), @"
by: qwe {param1} {param2}
");

            var component = gc.Components[0];
            component.By.Should().NotBeNull();
            component.By.Value.Should().Be("qwe {param1} {param2}");
            component.By.Segments.Should().HaveCount(2);
            component.By.Segments[0].Should().Be("param1");
            component.By.Segments[1].Should().Be("param2");
        }
    }
}
