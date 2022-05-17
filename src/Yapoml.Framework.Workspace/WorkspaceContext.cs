using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yapoml.Framework.Workspace.Parsers;

namespace Yapoml.Framework.Workspace
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
                var pages = Parser.ParsePages(filePath);

                for (int i = 0; i < pages.Count; i++)
                {
                    var page = pages[i];

                    var fileName = Path.GetFileName(filePath);
                    var pageName = fileName.Substring(0, fileName.Length - ".po.yaml".Length);

                    // adjust page family
                    if (i != 0)
                    {
                        pageName = $"{pageName}_{i}";
                    }

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

                    // find referencing components
                    if (pageContext.Components != null)
                    {
                        foreach (var componentContext in pageContext.Components)
                        {
                            FindReferencingComponents(_referencingComponents, componentContext);
                        }
                    }
                }
            }
            else if (filePath.ToLowerInvariant().EndsWith(".pc.yaml"))
            {
                var component = Parser.ParseComponent(filePath);

                var fileName = Path.GetFileName(filePath);

                var componentName = fileName.Substring(0, fileName.Length - ".pc.yaml".Length);

                ComponentContext componentContext;

                if (space == null)
                {
                    componentContext = new ComponentContext(componentName, this, null, null, component);

                    Components.Add(componentContext);
                }
                else
                {
                    componentContext = new ComponentContext(componentName, this, space, null, component);

                    space.Components.Add(componentContext);
                }

                FindReferencingComponents(_referencingComponents, componentContext);
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
            ResolveReferencingComponents();
        }

        private readonly IDictionary<PageContext, string> _inheritedPages = new Dictionary<PageContext, string>();
        private readonly IDictionary<ComponentContext, string> _referencingComponents = new Dictionary<ComponentContext, string>();

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

        private void FindReferencingComponents(IDictionary<ComponentContext, string> referencingComponents, ComponentContext component)
        {
            if (component.ReferencedComponentName != null)
            {
                referencingComponents.Add(component, component.ReferencedComponentName);
            }

            if (component.Components != null)
            {
                foreach (var c in component.Components)
                {
                    FindReferencingComponents(referencingComponents, c);
                }
            }
        }

        private void ResolveReferencingComponents()
        {
            // todo: reimplement it to not travel through entire tree again and again
            foreach (var referencingComponent in _referencingComponents)
            {
                referencingComponent.Key.ReferencedComponent = DiscoverComponents(this.Components, referencingComponent.Value);

                if (referencingComponent.Key.ReferencedComponent == null)
                {
                    if (this.Pages != null)
                    {
                        foreach (var pageContext in this.Pages)
                        {
                            referencingComponent.Key.ReferencedComponent = DiscoverComponents(pageContext.Components, referencingComponent.Value);

                            if (referencingComponent.Key.ReferencedComponent != null)
                            {
                                break;
                            }
                        }
                    }

                    if (referencingComponent.Key.ReferencedComponent == null)
                    {
                        if (this.Spaces != null)
                        {
                            foreach (var space in this.Spaces)
                            {
                                referencingComponent.Key.ReferencedComponent = DiscoverSpaceForRefComponent(space, referencingComponent.Value);

                                if (referencingComponent.Key.ReferencedComponent != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                if (referencingComponent.Key.ReferencedComponent == null)
                {
                    throw new Exception($"Cannot resolve referenced '{referencingComponent.Value}' component for '{referencingComponent.Key.Name}'.");
                }

                // implicitly use By from referenced component
                if (referencingComponent.Key.By == null)
                {
                    referencingComponent.Key.By = referencingComponent.Key.ReferencedComponent.By;
                }
            }
        }

        private ComponentContext DiscoverSpaceForRefComponent(SpaceContext space, string refName)
        {
            var component = DiscoverComponents(space.Components, refName);

            if (component == null)
            {
                if (space.Pages != null)
                {
                    foreach (var page in space.Pages)
                    {
                        component = DiscoverComponents(page.Components, refName);

                        if (component != null)
                        {
                            break;
                        }
                    }
                }

                if (component == null)
                {
                    if (space.Spaces != null)
                    {
                        foreach (var s in space.Spaces)
                        {
                            component = DiscoverSpaceForRefComponent(s, refName);
                        }
                    }
                }
            }

            return component;
        }

        private ComponentContext DiscoverComponents(IList<ComponentContext> components, string refName)
        {
            if (components == null)
            {
                return null;
            }

            foreach (var component in components)
            {
                if (EvaluateComponent(component, refName))
                {
                    return component;
                }
                else
                {
                    return DiscoverComponents(component.Components, refName);
                }
            }

            return null;
        }

        private bool EvaluateComponent(ComponentContext component, string refName)
        {
            return component.Name.Equals(refName, StringComparison.OrdinalIgnoreCase);
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
