using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using Yapoml.Framework.Workspace.Parsers;
using Yapoml.Framework.Workspace;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Test.Workspace
{
    internal class PageTests
    {
        private WorkspaceParser _parser = new WorkspaceParser();
        private INameNormalizer _nameNormalizer = new NameNormalizer();

        [Test]
        public void Parse_Page()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_page.page.yaml"), @"
C1:
  by: qwe
");

            gc.Spaces.Should().BeEmpty();

            gc.Pages.Should().HaveCount(1);
            var page = gc.Pages[0];
            page.Name.Should().Be("MyPage");
            page.Namespace.Should().Be("A.B");

            gc.Pages[0].Components.Should().HaveCount(1);

            gc.Pages[0].Url.Should().BeNull();

            gc.Pages[0].RelativeFilePath.Should().Be("my_page.page.yaml");
        }

        [Test]
        public void Parse_Page_Url()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_page.page.yml"), @"
url:
  path: projects/{projectId}/users/{userId}/roles
  params:
    - count
    - offset
");

            var url = gc.Pages[0].Url;

            url.Path.Should().Be("projects/{projectId}/users/{userId}/roles");
            url.Params.Should().HaveCount(2);

            var countParam = url.Params[0];
            countParam.Should().Be("count");


            var offsetParam = url.Params[1];
            offsetParam.Should().Be("offset");

            url.Segments.Should().HaveCount(2);

            var projectSegment = url.Segments[0];
            projectSegment.Should().Be("projectId");

            var userIdSegment = url.Segments[1];
            userIdSegment.Should().Be("userId");
        }

        [Test]
        public void Parse_Pages()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser, new WorkspaceReferenceResolver(), _nameNormalizer);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_page.page.yaml"), @"
C1:
  by: qwe

---

C2:
  by: asd
");

            gc.Pages.Should().HaveCount(2);

            gc.Pages[0].Name.Should().Be("MyPage");
            gc.Pages[1].Name.Should().Be("MyPage1");
        }
    }
}
