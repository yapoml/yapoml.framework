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
params:
";
            var url = _parser.Parse<Url>(content);
            url.Path.Should().Be("some/path");

            url.Params.Should().BeEmpty();
        }

        [Test]
        public void Should_Parse_Several_Query()
        {
            var content = @"
path: some/path
params:
  - param1
  - param2
";
            var url = _parser.Parse<Url>(content);
            url.Path.Should().Be("some/path");

            url.Params.Should().NotBeNull();
            url.Params.Should().HaveCount(2);

            url.Params[0].Should().Be("param1");
            url.Params[1].Should().Be("param2");
        }
    }
}