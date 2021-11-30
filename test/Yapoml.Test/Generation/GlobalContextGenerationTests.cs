using FluentAssertions;
using Moq;
using NUnit.Framework;
using Yapoml.Generation;
using Yapoml.Parsers;

namespace Yapoml.Test.Generation
{
    internal class GlobalContextGenerationTests
    {
        Mock<IComponentParser> _componentParser;

        [SetUp]
        public void SetUp()
        {
            _componentParser = new Mock<IComponentParser>();
            _componentParser.Setup((cp) => cp.Parse(It.IsAny<string>())).Returns(new Parsers.Yaml.Pocos.Component());
        }

        [Test]
        public void Add_Files()
        {
            var gc = new GlobalGenerationContext("/some/path", "A.B", _componentParser.Object);

            gc.AddFile("/some/path/any/other/file1.po.yaml");
            gc.AddFile("/some/path/any/other/file2.po.yaml");

            gc.Spaces.Should().HaveCount(1);

            var anySpace = gc.Spaces[0];
            anySpace.Name.Should().Be("any");
            anySpace.Namespace.Should().Be("A.B.any");
            anySpace.Spaces.Should().HaveCount(1);

            var otherSpace = anySpace.Spaces[0];
            otherSpace.Name.Should().Be("other");
            otherSpace.Namespace.Should().Be("A.B.any.other");

            otherSpace.Components.Should().HaveCount(2);
        }

        [Test]
        public void Add_Root_File()
        {
            var gc = new GlobalGenerationContext("/some/path", "A.B", _componentParser.Object);

            gc.AddFile("/some/path/file.po.yaml");

            gc.RootNamespace.Should().Be("A.B");
            gc.RootDirectoryPath.Should().Be("\\some\\path");

            gc.Spaces.Should().HaveCount(0);

            gc.Components.Should().HaveCount(1);
        }
    }
}
