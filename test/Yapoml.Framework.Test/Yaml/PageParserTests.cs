using FluentAssertions;
using NUnit.Framework;
using Yapoml.Framework.Workspace.Parsers.Yaml;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Framewok.Test.Yaml
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

            page.BasePage.Should().BeNull();

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
            page.BasePage.Should().BeNull();
        }

        [Test]
        public void Should_Parse_Url()
        {
            var content = @"
url: /some/path
";

            var page = _parser.Parse<Page>(content);
            page.Url.Should().NotBeNull();
            page.Url.Path.Should().Be("/some/path");
        }

        [Test]
        public void Should_Parse_Extends()
        {
            var content = @"
extends: SomeBasePage
";

            var page = _parser.Parse<Page>(content);
            page.BasePage.Should().Be("SomeBasePage");
        }

        [Test]
        public void Should_Parse_Base()
        {
            var content = @"
base: SomeBasePage
";

            var page = _parser.Parse<Page>(content);
            page.BasePage.Should().Be("SomeBasePage");
        }

        [Test]
        public void Should_Parse_EmptyExtends()
        {
            var content = @"
extends:
";

            var page = _parser.Parse<Page>(content);
            page.BasePage.Should().BeEmpty();
        }

        [Test]
        public void Should_Parse_VerySimplifiedPage()
        {
            var content = @"
C1: css qwe
";

            var page = _parser.Parse<Page>(content);

            var component = page.Components[0];
            component.Name.Should().Be("C1");
            component.By.Method.Should().Be(By.ByMethod.Css);
            component.By.Value.Should().Be("qwe");
        }
    }
}
