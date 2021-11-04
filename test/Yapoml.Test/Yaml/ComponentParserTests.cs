using FluentAssertions;
using NUnit.Framework;
using Yapoml.Parsers.Yaml;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Test.Yaml
{
    public class ComponentParserTests
    {
        private readonly YamlParser _parser = new();

        [Test]
        public void Should_Parse_Component()
        {
            var content = @"
name: C1
by: ./abc
";
            var component = _parser.Parse<Component>(content);
            component.Name.Should().Be("C1");
            component.By.Should().NotBeNull();
        }

        [Test]
        public void Should_Parse_Nested_Components()
        {
            var content = @"
name: C1
by: ./abc

ya:
  Component2:
    name: C2

  Component3: { name: C3 }
";
            var component = _parser.Parse<Component>(content);
            component.Name.Should().Be("C1");
            component.By.Should().NotBeNull();

            var nestedComponents = component.NestedComponents;
            nestedComponents.Should().HaveCount(2);
            var nested2 = nestedComponents["Component2"];
            nested2.Name.Should().Be("C2");
            var nested3 = nestedComponents["Component3"];
            nested3.Name.Should().Be("C3");
        }
    }
}
