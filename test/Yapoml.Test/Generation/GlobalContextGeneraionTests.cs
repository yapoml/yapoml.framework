using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yapoml.Generation;

namespace Yapoml.Test.Generation
{
    internal class GlobalContextGeneraionTests
    {
        [Test]
        public void AddFile()
        {
            var gc = new GlobalGenerationContext("/some/path", "A.B");

            gc.AddFile("/some/path/any/other/file.po.yaml");
        }

        [Test]
        public void AddFile2()
        {
            var gc = new GlobalGenerationContext("/some/path", "A.B");

            gc.AddFile("/some/path/file.po.yaml");
        }
    }
}
