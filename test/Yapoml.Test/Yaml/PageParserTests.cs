using FluentAssertions;
using NUnit.Framework;
using Yapoml.Parsers.Yaml;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Test.Yaml
{
    public class PageParserTests
    {
        private readonly YamlParser _parser = new();

        [Test]
        public void Should_Parse_Page()
        {
            var content = @"
ya:
  Component1:
    name: C1

  Component2: { name: C2 }
";
            var page = _parser.Parse<Page>(content);
            
            var components = page.Components;
            components.Should().HaveCount(2);
            var nested1 = components["Component1"];
            nested1.Name.Should().Be("C1");
            var nested2 = components["Component2"];
            nested2.Name.Should().Be("C2");
        }
    }
}
