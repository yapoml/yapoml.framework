using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using Yapoml.Generation;
using Yapoml.Parsers;

namespace Yapoml.Test.Generation
{
    internal class ComponentContextGenerationTests
    {
        private Parser _parser = new Parser();

        [Test]
        public void Parse_Component()
        {
            File.WriteAllText("my_component.pc.yaml", @"
name: c1
by: qwe

ya:
  c2:
    by: asd
"
                );

            var gc = new GlobalGenerationContext(Environment.CurrentDirectory, "A.B", _parser);

            gc.AddFile($"{Environment.CurrentDirectory}\\my_component.pc.yaml");

            gc.Spaces.Should().BeEmpty();

            gc.Components.Should().HaveCount(1);

            var component = gc.Components[0];
            component.Name.Should().Be("c1");

            component.ComponentGenerationContextes[0].Name.Should().Be("c2");
        }

        [Test]
        public void Component_Name_Should_Be_Optional()
        {
            File.WriteAllText("my_component.pc.yaml", @"
by: qwe
"
                );

            var gc = new GlobalGenerationContext(Environment.CurrentDirectory, "A.B", _parser);

            gc.AddFile($"{Environment.CurrentDirectory}\\my_component.pc.yaml");

            gc.Spaces.Should().BeEmpty();

            gc.Components.Should().HaveCount(1);

            var component = gc.Components[0];
            component.Name.Should().Be("my_component");
        }
    }
}
