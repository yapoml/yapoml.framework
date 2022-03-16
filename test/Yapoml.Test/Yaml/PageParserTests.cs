using FluentAssertions;
using NUnit.Framework;
using Yapoml.Generation.Parsers.Yaml;
using Yapoml.Generation.Parsers.Yaml.Pocos;

namespace Yapoml.Test.Yaml
{
    public class PageParserTests
    {
        private readonly YamlParser _parser = new();

        [Test]
        public void Should_Parse_Page()
        {
            var content = @"
Component1:
  by: qwe

Component2: { by: asd }
";
            var page = _parser.Parse<Page>(content);

            var components = page.Components;
            components.Should().HaveCount(2);
            var nested1 = components[0];
            nested1.Name.Should().Be("Component1");
            var nested2 = components[1];
            nested2.Name.Should().Be("Component2");
        }

        [Test]
        public void Should_Parse_EmptyPage()
        {
            var content = "";

            var page = _parser.Parse<Page>(content);
            page.Components.Should().BeNull();
        }
    }
}
