using FluentAssertions;
using Moq;
using NUnit.Framework;
using Yapoml.Generation;
using Yapoml.Parsers;

namespace Yapoml.Test.Generation
{
    internal class GlobalContextGenerationTests
    {
        Mock<IParser> _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new Mock<IParser>();
            _parser.Setup((cp) => cp.ParsePage(It.IsAny<string>())).Returns(new Parsers.Yaml.Pocos.Page());
            _parser.Setup((cp) => cp.ParseComponent(It.IsAny<string>())).Returns(new Parsers.Yaml.Pocos.Component());
        }

        [Test]
        public void Add_Files()
        {
            var gc = new GlobalGenerationContext("/some/path", "A.B", _parser.Object);

            gc.AddFile("/some/path/any/other/file1.po.yaml");
            gc.AddFile("/some/path/any/other/file2.po.yaml");

            gc.AddFile("/some/path/any/c1.pc.yaml");

            gc.Spaces.Should().HaveCount(1);

            var anySpace = gc.Spaces[0];
            anySpace.Name.Should().Be("any");
            anySpace.Namespace.Should().Be("A.B.any");
            anySpace.Spaces.Should().HaveCount(1);

            anySpace.Components.Should().HaveCount(1);

            var c1Component = anySpace.Components[0];
            c1Component.Name.Should().Be("c1");

            var otherSpace = anySpace.Spaces[0];
            otherSpace.Name.Should().Be("other");
            otherSpace.Namespace.Should().Be("A.B.any.other");

            otherSpace.Pages.Should().HaveCount(2);
            otherSpace.Pages[0].Name.Should().Be("file1");
            otherSpace.Pages[1].Name.Should().Be("file2");
        }

        [Test]
        public void Add_Root_File()
        {
            var gc = new GlobalGenerationContext("/some/path", "A.B", _parser.Object);

            gc.AddFile("/some/path/file.po.yaml");

            gc.RootNamespace.Should().Be("A.B");
            gc.RootDirectoryPath.Should().Be("\\some\\path");

            gc.Spaces.Should().HaveCount(0);

            gc.Pages.Should().HaveCount(1);
        }
    }
}
