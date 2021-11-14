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
        }

        [Test]
        public void AddRootFile()
        {
            var gc = new GlobalGenerationContext("/some/path", "A.B");

            gc.AddFile("/some/path/file.po.yaml");
        }
    }
}
