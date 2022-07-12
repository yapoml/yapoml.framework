using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using Yapoml.Framework.Workspace.Parsers;
using Yapoml.Framework.Workspace;

namespace Yapoml.Framewok.Test.Workspace
{
    internal class PageTests
    {
        private WorkspaceParser _parser = new WorkspaceParser();

        [Test]
        public void Parse_Page()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_page.po.yaml"), @"
C1:
  by: qwe
");

            gc.Spaces.Should().BeEmpty();

            gc.Pages.Should().HaveCount(1);
            var page = gc.Pages[0];
            page.Name.Should().Be("my_page");
            page.Namespace.Should().Be("A.B");

            gc.Pages[0].Components.Should().HaveCount(1);

            gc.Pages[0].Url.Should().BeNull();
        }

        [Test]
        public void Parse_Page_Url()
        {
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_page.po.yaml"), @"
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
            var gc = new WorkspaceContext(Environment.CurrentDirectory, "A.B", _parser);

            gc.AddFile(Path.Combine(Environment.CurrentDirectory, "my_page.po.yaml"), @"
C1:
  by: qwe

---

C2:
  by: asd
");

            gc.Pages.Should().HaveCount(2);

            gc.Pages[0].Name.Should().Be("my_page");
            gc.Pages[1].Name.Should().Be("my_page_1");
        }
    }
}
