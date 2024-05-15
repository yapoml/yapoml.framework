using FluentAssertions;
using NUnit.Framework;
using System;
using Yapoml.Framework.Workspace.Parsers.Yaml;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;
using static Yapoml.Framework.Workspace.Parsers.Yaml.Pocos.By;

namespace Yapoml.Framework.Test.Yaml
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
            by.Region.Start.Should().Be(new Framework.Workspace.Parsers.Region.Position(1, 8));
            by.Region.End.Should().Be(new Framework.Workspace.Parsers.Region.Position(1, 12));
        }

        [Test]
        public void Should_Parse_By_Mapping_TestId()
        {
            var content = @"testid: abc";
            var by = _parser.Parse<By>(content);
            by.Method.Should().Be(ByMethod.TestId);
            by.Value.Should().Be("abc");
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

        [Test]
        public void Scope_Should_BeParentByDefault()
        {
            var content = $"xpath: .//div{Environment.NewLine}from: parent";

            var by = _parser.Parse<By>(content);

            by.Method.Should().Be(ByMethod.XPath);
            by.Value.Should().Be(".//div");
            by.Scope.Should().Be(ByScope.Parent);
        }

        [Test]
        public void Scope_Should_BeParent()
        {
            var content = "fRoM: paRenT";

            var by = _parser.Parse<By>(content);

            by.Scope.Should().Be(ByScope.Parent);
        }

        [Test]
        public void Scope_Should_BeRoot()
        {
            var content = "fRoM: rOoT";

            var by = _parser.Parse<By>(content);

            by.Scope.Should().Be(ByScope.Root);
        }
    }
}