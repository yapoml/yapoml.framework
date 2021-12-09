using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using Yapoml.Generation;
using Yapoml.Parsers;

namespace Yapoml.Test.Generation
{
    internal class PageContextGenerationTests
    {
        private Parser _parser = new Parser();

        [Test]
        public void Parse_Page()
        {
            File.WriteAllText("my_page.po.yaml", @"
ya:
  C1:
    by: qwe
"
                );

            var gc = new GlobalGenerationContext(Environment.CurrentDirectory, "A.B", _parser);

            gc.AddFile($"{Environment.CurrentDirectory}\\my_page.po.yaml");

            gc.Spaces.Should().BeEmpty();

            gc.Pages.Should().HaveCount(1);

            gc.Pages[0].Components.Should().HaveCount(1);
        }
    }
}
