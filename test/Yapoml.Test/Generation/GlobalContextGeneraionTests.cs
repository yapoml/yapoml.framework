using FluentAssertions;
using NUnit.Framework;
using Yapoml.Generation;

namespace Yapoml.Test.Generation
{
    internal class GlobalContextGeneraionTests
    {
        [Test]
        public void AddFiles()
        {
            var gc = new GlobalGenerationContext("/some/path", "A.B");

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
        }

        [Test]
        public void AddRootFile()
        {
            var gc = new GlobalGenerationContext("/some/path", "A.B");

            gc.AddFile("/some/path/file.po.yaml");

            gc.RootNamespace.Should().Be("A.B");
            gc.RootDirectoryPath.Should().Be("\\some\\path");

            gc.Spaces.Should().HaveCount(0);
        }
    }
}
