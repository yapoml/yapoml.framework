using FluentAssertions;
using NUnit.Framework;
using Yapoml.Framework.Workspace.Parsers.Yaml;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Framewok.Test.Yaml
{
    public class ComponentParserTests
    {
        private readonly YamlParser _parser = new();

        [Test]
        public void Should_Parse_Component()
        {
            var content = @"
by: abc
";
            var component = _parser.Parse<Component>(content);
            component.Name.Should().BeNull();
            component.By.Should().NotBeNull();
            component.By.Method.Should().Be(By.ByMethod.None);
            component.By.Value.Should().Be("abc");
            component.Ref.Should().BeNull();
        }

        [Test]
        public void Should_Parse_Nested_Components()
        {
            var content = @"
by: css .abc

Component2:
  by: c2

Component3: { by: c3 }
";
            var component = _parser.Parse<Component>(content);
            component.Name.Should().BeNull();
            component.By.Should().NotBeNull();

            var nestedComponents = component.Components;
            nestedComponents.Should().HaveCount(2);
            var nested2 = nestedComponents[0];
            nested2.Name.Should().Be("Component2");
            var nested3 = nestedComponents[1];
            nested3.Name.Should().Be("Component3");
        }

        [Test]
        public void Should_Parse_Ref()
        {
            var content = @"
ref: abc
";
            var component = _parser.Parse<Component>(content);

            component.Ref.Should().Be("abc");
        }

        [Test]
        [TestCase("base")]
        [TestCase("Base")]
        [TestCase("extends")]
        [TestCase("Extends")]
        public void Should_Parse_Base(string keyword)
        {
            var content = @$"
{keyword}: abc
";
            var component = _parser.Parse<Component>(content);

            component.BaseComponent.Should().Be("abc");
        }
    }
}
