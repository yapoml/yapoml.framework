using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using Yapoml.Workspace;
using Yapoml.Workspace.Parsers;

namespace Yapoml.Test.Workspace
{
    internal class ComponentTests
    {
        private WorkspaceParser _parser = new WorkspaceParser();

        [Test]
        public void Parse_Component()
        {
            File.WriteAllText("my_component.pc.yaml", @"
by: qwe

c2:
  by: asd
"
                );

            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.pc.yaml"));

            gc.Spaces.Should().BeEmpty();

            gc.Components.Should().HaveCount(1);

            var component = gc.Components[0];
            component.Name.Should().Be("my_component");

            component.Components[0].Name.Should().Be("c2");
        }

        [Test]
        public void Component_Name_Should_Be_Optional()
        {
            File.WriteAllText("my_component.pc.yaml", @"
by: qwe
"
                );

            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_component.pc.yaml"));

            gc.Spaces.Should().BeEmpty();

            gc.Components.Should().HaveCount(1);

            var component = gc.Components[0];
            component.Name.Should().Be("my_component");
        }
    }
}
