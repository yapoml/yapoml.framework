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

        [Test]
        public void Parse_Component()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver());

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.pc.yaml"), @"
by: qwe

c2:
  by: asd
");

            gc.Spaces.Should().BeEmpty();

            gc.Components.Should().HaveCount(1);

            var component = gc.Components[0];
            component.Name.Should().Be("my_component");

            component.Components[0].Name.Should().Be("c2");
        }

        [Test]
        public void Component_Name_Should_Be_Optional()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver());

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.pc.yaml"), @"
by: qwe
");

            gc.Spaces.Should().BeEmpty();

            gc.Components.Should().HaveCount(1);

            var component = gc.Components[0];
            component.Name.Should().Be("my_component");
        }

        [Test]
        public void Component_Segment()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver());

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
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver());

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
