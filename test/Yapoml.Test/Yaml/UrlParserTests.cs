using FluentAssertions;
using NUnit.Framework;
using Yapoml.Generation.Parsers.Yaml;
using Yapoml.Generation.Parsers.Yaml.Pocos;

namespace Yapoml.Test.Yaml
{
    public class UrlParserTests
    {
        private readonly YamlParser _parser = new();

        [Test]
        public void Should_Parse_Scalar_Path()
        {
            var content = @"some/path";
            var url = _parser.Parse<Url>(content);
            url.Path.Should().Be("some/path");
        }

        [Test]
        public void Should_Parse_Named_Path()
        {
            var content = @"
path: some/path
";
            var url = _parser.Parse<Url>(content);
            url.Path.Should().Be("some/path");
        }

        [Test]
        public void Should_Parse_Query()
        {
            var content = @"
path: some/path
query:
";
            var url = _parser.Parse<Url>(content);
            url.Path.Should().Be("some/path");

            url.QueryParams.Should().BeEmpty();
        }

        [Test]
        public void Should_Parse_Several_Query()
        {
            var content = @"
path: some/path
query:
  - param1
  - param2
";
            var url = _parser.Parse<Url>(content);
            url.Path.Should().Be("some/path");

            url.QueryParams.Should().NotBeNull();
            url.QueryParams.Should().HaveCount(2);

            url.QueryParams[0].Name.Should().Be("param1");
            url.QueryParams[1].Name.Should().Be("param2");
        }

        [Test]
        public void Should_Parse_Optional_Query()
        {
            var content = @"
path: some/path
query:
  - name: param1
    optional: no
";
            var url = _parser.Parse<Url>(content);
            url.Path.Should().Be("some/path");

            url.QueryParams.Should().NotBeNull();
            url.QueryParams.Should().HaveCount(1);

            url.QueryParams[0].Name.Should().Be("param1");
            url.QueryParams[0].IsOptional.Should().Be(false);
        }

        [Test]
        public void Should_Skip_Unknown_QueryProperties()
        {
            var content = @"
path: some/path
query:
  - name: param1
    unknown: [a, b, c]
    optional: no
";
            var url = _parser.Parse<Url>(content);
            url.Path.Should().Be("some/path");

            url.QueryParams.Should().NotBeNull();
            url.QueryParams.Should().HaveCount(1);

            url.QueryParams[0].Name.Should().Be("param1");
            url.QueryParams[0].IsOptional.Should().Be(false);
        }
    }
}