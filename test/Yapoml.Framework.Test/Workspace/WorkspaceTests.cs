using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using Yapoml.Framework.Workspace.Parsers;
using Yapoml.Framework.Workspace;
using System.Collections.Generic;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Test.Workspace
{
    internal class WorkspaceTests
    {
        private Mock<IWorkspaceParser> _parser;
        private INameNormalizer _nameNormalizer = new NameNormalizer();

        [SetUp]
        public void SetUp()
        {
            _parser = new Mock<IWorkspaceParser>();
            _parser.Setup((cp) => cp.ParsePages(It.IsAny<string>())).Returns(() => new List<Framework.Workspace.Parsers.Yaml.Pocos.Page> { new Framework.Workspace.Parsers.Yaml.Pocos.Page() });
            _parser.Setup((cp) => cp.ParseComponent(It.IsAny<string>())).Returns(() => new Framework.Workspace.Parsers.Yaml.Pocos.Component());
        }

        [Test]
        public void Add_Files()
        {
            var gc = new WorkspaceContext("/some/path", "A.B", _parser.Object, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile("/some/path/any/other/file1.po.yaml", "");
            gc.AddFile("/some/path/any/other/file2.po.yaml", "");

            gc.AddFile("/some/path/any/c1.pc.yaml", "");

            gc.Spaces.Should().HaveCount(1);

            var anySpace = gc.Spaces[0];
            anySpace.Name.Should().Be("Any");
            anySpace.Namespace.Should().Be("A.B.Any");
            anySpace.Spaces.Should().HaveCount(1);

            anySpace.Components.Should().HaveCount(1);

            var c1Component = anySpace.Components[0];
            c1Component.Name.Should().Be("C1");

            var otherSpace = anySpace.Spaces[0];
            otherSpace.Name.Should().Be("Other");
            otherSpace.Namespace.Should().Be("A.B.Any.Other");

            otherSpace.Pages.Should().HaveCount(2);
            otherSpace.Pages[0].Name.Should().Be("File1");
            otherSpace.Pages[1].Name.Should().Be("File2");
        }

        [Test]
        public void Add_Root_File()
        {
            var gc = new WorkspaceContext("/some/path", "A.B", _parser.Object, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile("/some/path/file.po.yaml", "");

            gc.RootNamespace.Should().Be("A.B");
            gc.RootDirectoryPath.Should().Be("\\some\\path");

            gc.Spaces.Should().HaveCount(0);

            gc.Pages.Should().HaveCount(1);
        }

        [Test]
        public void Should_Resolve_Inheritance()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", new WorkspaceParser(), new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Environment.CurrentDirectory + "/MyBasePage.po.yaml", @"
MyBaseComp: ./a
");

            gc.AddFile(Environment.CurrentDirectory + "/MyPage.po.yaml", @"
base: mybasepage

MyComp:
  base: MyBaseComp
  by: ./b
");

            gc.AddFile(Environment.CurrentDirectory + "/MySecondPage.po.yaml", @"
extends: mybasepage
");

            gc.ResolveReferences();

            gc.Pages[1].BasePage.Should().Be(gc.Pages[0]);
            gc.Pages[2].BasePage.Should().Be(gc.Pages[0]);

            gc.Pages[1].Components[0].BaseComponent.Should().Be(gc.Pages[0].Components[0]);
        }

        [Test]
        public void Should_Resolve_ReferencedComponent_InPage()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", new WorkspaceParser(), new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Environment.CurrentDirectory + "/MyPage.po.yaml", @"
C1: qwe
C2:
  ref: C1
");

            gc.ResolveReferences();

            gc.Pages[0].Components[1].ReferencedComponent.Should().BeSameAs(gc.Pages[0].Components[0]);
        }

        [Test]
        public void Should_Resolve_ReferencedComponent_InSpace()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", new WorkspaceParser(), new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Environment.CurrentDirectory + "/MyComponent.pc.yaml", "");

            gc.AddFile(Environment.CurrentDirectory + "/MyPage.po.yaml", @"
C2:
  ref: MyComponent
");

            gc.ResolveReferences();

            gc.Pages[0].Components[0].ReferencedComponent.Should().BeSameAs(gc.Components[0]);
        }

        [Test]
        public void Should_Throw_Resolve_Inheritance_IfNotFound()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", new WorkspaceParser(), new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Environment.CurrentDirectory + "/MyPage.po.yaml", @"
base: mybasepage
");

            Action act = () => gc.ResolveReferences();
            act.Should().Throw<Exception>().And.Message.Should().Contain("MyPage");
        }

        [Test]
        public void Should_Throw_Resolve_References_IfNotFound()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", new WorkspaceParser(), new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Environment.CurrentDirectory + "/MyPage.po.yaml", @"
MyComponent:
  ref: UnknownComponent
");

            Action act = () => gc.ResolveReferences();
            act.Should().Throw<Exception>().And.Message.Should().Contain("UnknownComponent");
        }

        [Test]
        [TestCase("with space", "WithSpace")]
        [TestCase("with-dash", "WithDash")]
        public void Should_Normalize_PageName(string pageName, string expectedPageName)
        {
            var gc = new WorkspaceContext("/some/path", "A.B", _parser.Object, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile($"/some/path/{pageName}.po.yaml", "");

            gc.Pages[0].Name.Should().Be(expectedPageName);
        }

        [Test]
        [TestCase("with space", "WithSpace")]
        [TestCase("with-dash", "WithDash")]
        public void Should_Normalize_SpaceName(string spaceName, string expectedSpaceName)
        {
            var gc = new WorkspaceContext("/some/path", "A.B", _parser.Object, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile($"/some/path/{spaceName}/page.po.yaml", "");

            gc.Spaces[0].Name.Should().Be(expectedSpaceName);
        }
    }
}
