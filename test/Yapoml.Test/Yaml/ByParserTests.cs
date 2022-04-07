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
        public void Should_Parse_By_Scalar_None_AsDefault()
        {
            var content = @"./abc";
            var by = _parser.Parse<By>(content);
            by.Method.Should().Be(ByMethod.None);
            by.Value.Should().Be("./abc");
        }

        [Test]
        public void Should_Throw_By_Unknown_Mapping()
        {
            var content = @"unknown_by_type: ./abc";
            Action action = () => _parser.Parse<By>(content);

            action.Should().Throw<Exception>().WithInnerException<Exception>().WithMessage("Cannot map*");
        }

        [Test]
        [TestCase("by xpath ./a", "./a", ByMethod.XPath)]
        [TestCase("BY xPath ./a", "./a", ByMethod.XPath)]
        [TestCase("xpath ./a", "./a", ByMethod.XPath)]
        [TestCase("xPath ./a", "./a", ByMethod.XPath)]
        public void Should_Parse_By_Scalar(string value, string expectedValue, ByMethod expectedMethod)
        {
            var content = $"{value}";

            var by = _parser.Parse<By>(content);

            by.Method.Should().Be(expectedMethod);
            by.Value.Should().Be(expectedValue);
        }
    }
}