using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using Yapoml.Generation;
using Yapoml.Parsers;

namespace Yapoml.Test.Generation
{
    internal class ComponentContextGenerationTests
    {
        private ComponentParser _componentParser = new ComponentParser();

        [Test]
        public void Parse_Component()
        {
            File.WriteAllText("my_page.po.yaml", @"
name: c1
by: qwe
"
                );

            var gc = new GlobalGenerationContext(Environment.CurrentDirectory, "A.B", _componentParser);

            gc.AddFile($"{Environment.CurrentDirectory}\\my_page.po.yaml");

            gc.Spaces.Should().BeEmpty();

            gc.Components.Should().HaveCount(1);
        }
    }
}
