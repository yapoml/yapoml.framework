using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yapoml.Workspace.Parsers;

namespace Yapoml.Workspace
{
    public class WorkspaceContext
    {
        public WorkspaceContext(string rootDirectoryPath, string rootNamespace, IWorkspaceParser parser)
        {
            RootDirectoryPath = rootDirectoryPath.Replace("/", "\\").TrimEnd('\\');
            RootNamespace = rootNamespace;
            Parser = parser;
        }

        private IWorkspaceParser Parser { get; }

        public string RootDirectoryPath { get; }

        public string RootNamespace { get; }

        public IList<SpaceContext> Spaces { get; } = new List<SpaceContext>();

        public IList<PageContext> Pages { get; } = new List<PageContext>();

        public IList<ComponentContext> Components { get; } = new List<ComponentContext>();

        public void AddFile(string filePath)
        {
            var space = CreateOrAddSpaces(filePath);

            if (filePath.ToLowerInvariant().EndsWith(".po.yaml"))
            {
                var page = Parser.ParsePage(filePath);
                var fileName = Path.GetFileName(filePath);
                var pageName = fileName.Substring(0, fileName.Length - ".po.yaml".Length);

                PageContext pageContext;

                if (space == null)
                {
                    pageContext = new PageContext(pageName, this, null, page);

                    Pages.Add(pageContext);
                }
                else
                {
                    pageContext = new PageContext(pageName, this, space, page);

                    space.Pages.Add(pageContext);
                }

                if (page.BasePage != null)
                {
                    _inheritedPages.Add(pageContext, page.BasePage);
                }
            }
            else if (filePath.ToLowerInvariant().EndsWith(".pc.yaml"))
            {
                var component = Parser.ParseComponent(filePath);

                var fileName = Path.GetFileName(filePath);

                var componentName = fileName.Substring(0, fileName.Length - ".pc.yaml".Length);

                if (space == null)
                {
                    var componentContext = new ComponentContext(componentName, this, null, component);

                    Components.Add(componentContext);
                }
                else
                {
                    var componentContext = new ComponentContext(componentName, this, space, component);

                    space.Components.Add(componentContext);
                }
            }
        }

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString();
            }
        }

        public void ResolveReferences()
        {
            ResolveInheritedPages();
        }

        private readonly IDictionary<PageContext, string> _inheritedPages = new Dictionary<PageContext, string>();

        private void ResolveInheritedPages()
        {
            foreach (var pageContext in _inheritedPages)
            {
                var basePageName = pageContext.Value;
                var childPage = pageContext.Key;

                if (childPage.ParentSpace != null)
                {
                    var basePage = DiscoverSpace(childPage.ParentSpace, basePageName);

                    if (basePage != null)
                    {
                        childPage.BasePage = basePage;
                    }
                }
                else
                {
                    foreach (var page in Pages)
                    {
                        if (EvaluatePage(page, basePageName))
                        {
                            childPage.BasePage = page;
                        }
                    }
                }

                if (childPage.BasePage == null)
                {
                    throw new Exception($"Cannot resolve '{basePageName}' base page for '{childPage.Name}' page.");
                }
            }
        }

        private PageContext DiscoverSpace(SpaceContext space, string basePageName)
        {
            PageContext basePage = null;

            foreach (var page in space.Pages)
            {
                if (EvaluatePage(page, basePageName))
                {
                    return page;
                }
            }

            if (space.ParentSpace != null)
            {
                return DiscoverSpace(space.ParentSpace, basePageName);
            }

            return basePage;
        }

        private bool EvaluatePage(PageContext page, string basePageName)
        {
            return page.Name.Equals(basePageName, StringComparison.OrdinalIgnoreCase);
        }

        private SpaceContext CreateOrAddSpaces(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);

            var path = directory.Substring(RootDirectoryPath.Length);

            var parts = path.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 0)
            {
                SpaceContext nestedSpace = Spaces.FirstOrDefault(s => s.Namespace == $"{RootNamespace}.{parts[0]}");

                if (nestedSpace == null)
                {
                    nestedSpace = new SpaceContext(parts[0], this, null);

                    Spaces.Add(nestedSpace);
                }

                for (int i = 1; i < parts.Length; i++)
                {
                    var candidateNestedSpace = nestedSpace.Spaces.FirstOrDefault(s => s.Name == parts[i]);

                    if (candidateNestedSpace == null)
                    {
                        var newNestedSpace = new SpaceContext(parts[i], this, nestedSpace);

                        nestedSpace.Spaces.Add(newNestedSpace);

                        nestedSpace = newNestedSpace;
                    }
                    else
                    {
                        nestedSpace = candidateNestedSpace;
                    }
                }

                return nestedSpace;
            }
            else
            {
                return null;
            }
        }
    }
}
