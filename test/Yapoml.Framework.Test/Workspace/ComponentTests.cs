using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using Yapoml.Framework.Workspace.Parsers;
using Yapoml.Framework.Workspace;
using System.Linq;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framewok.Test.Workspace
{
    internal class ComponentTests
    {
        private WorkspaceParser _parser = new WorkspaceParser();
        private INameNormalizer _nameNormalizer = new NameNormalizer();

        [Test]
        public void Parse_Component()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.pc.yaml"), @"
by: qwe

c2:
  by: asd
");

            gc.Spaces.Should().BeEmpty();

            gc.Components.Should().HaveCount(1);

            var component = gc.Components[0];
            component.Name.Should().Be("MyComponent");
            component.RelativeFilePath.Should().Be("my_component.pc.yaml");

            component.Components[0].Name.Should().Be("C2");
            component.Components[0].RelativeFilePath.Should().Be("my_component.pc.yaml");
        }

        [Test]
        public void Component_Name_Should_Be_Optional()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.pc.yml"), @"
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

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.pc.yaml"), @"
by: qwe {param1}
");

            var component = gc.Components[0];
            component.By.Should().NotBeNull();
            component.By.Value.Should().Be("qwe {param1}");
            component.By.Segments.Should().HaveCount(1);
            component.By.Segments.First().Should().Be("param1");
        }

        [Test]
        public void Component_Segments()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.pc.yaml"), @"
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
