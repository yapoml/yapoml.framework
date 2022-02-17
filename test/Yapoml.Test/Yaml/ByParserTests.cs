using FluentAssertions;
using NUnit.Framework;
using System;
using Yapoml.Generation.Parsers.Yaml;
using Yapoml.Generation.Parsers.Yaml.Pocos;
using static Yapoml.Generation.Parsers.Yaml.Pocos.By;

namespace Yapoml.Test.Yaml
{
    public class ByParserTests
    {
        private readonly YamlParser _parser = new();

        [Test]
        public void Should_Parse_By_Mapping_Xpath()
        {
            var content = @"xpath: ./abc";
            var by = _parser.Parse<By>(content);
            by.Method.Should().Be(ByMethod.XPath);
            by.Value.Should().Be("./abc");
        }

        [Test]
        public void Should_Parse_By_Mapping_Xpath_CaseInsensitive()
        {
            var content = @"xpATh: ./abc";
            var by = _parser.Parse<By>(content);
            by.Method.Should().Be(ByMethod.XPath);
            by.Value.Should().Be("./abc");
        }

        [Test]
        public void Should_Parse_By_Scalar_Xpath_AsDefault()
        {
            var content = @"./abc";
            var by = _parser.Parse<By>(content);
            by.Method.Should().Be(ByMethod.XPath);
            by.Value.Should().Be("./abc");
        }

        [Test]
        public void Should_Throw_By_Unknown_Mapping()
        {
            var content = @"unknown_by_type: ./abc";
            Action action = () => _parser.Parse<By>(content);

            action.Should().Throw<Exception>().WithInnerException<Exception>().WithMessage("Cannot map*");
        }
    }
}