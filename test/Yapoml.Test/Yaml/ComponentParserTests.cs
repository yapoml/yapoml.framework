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
    }
}
