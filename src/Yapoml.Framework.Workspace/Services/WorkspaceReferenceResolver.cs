using System;
using System.Collections.Generic;
using System.Linq;

namespace Yapoml.Framework.Workspace.Services
{
    public class WorkspaceReferenceResolver : IWorkspaceReferenceResolver
    {
        private readonly IList<ComponentContext> _flatComponents = new List<ComponentContext>();

        private readonly IList<PageContext> _flatPages = new List<PageContext>();

        private readonly IDictionary<PageContext, string> _inheritedPages = new Dictionary<PageContext, string>();
        private readonly IDictionary<ComponentContext, string> _referencingComponents = new Dictionary<ComponentContext, string>();
        private readonly IDictionary<ComponentContext, string> _inheritedComponents = new Dictionary<ComponentContext, string>();

        public void AppendComponent(ComponentContext componentContext)
        {
            _flatComponents.Add(componentContext);

            if (componentContext.Components != null)
            {
                foreach (var innerComponent in componentContext.Components)
                {
                    AppendComponent(innerComponent);
                }
            }

            // whether we need resolve inherited component later
            if (componentContext.BaseComponentName != null)
            {
                _inheritedComponents.Add(componentContext, componentContext.BaseComponentName);
            }

            // whether we need resolve referencing component later
            if (componentContext.ReferencedComponentName != null)
            {
                _referencingComponents.Add(componentContext, componentContext.ReferencedComponentName);
            }
        }

        public void AppendPage(PageContext pageContext)
        {
            _flatPages.Add(pageContext);

            if (pageContext.Components != null)
            {
                foreach (var component in pageContext.Components)
                {
                    AppendComponent(component);
                }
            }

            if (pageContext.BasePageName != null)
            {
                _inheritedPages.Add(pageContext, pageContext.BasePageName);
            }
        }

        public void Resolve()
        {
            ResolveInheritedPages();
            ResolveReferencingComponents();
            ResolveInheritedComponents();
        }

        private void ResolveInheritedPages()
        {
            foreach (var pageContext in _inheritedPages)
            {
                var basePageName = pageContext.Value;
                var childPage = pageContext.Key;

                childPage.BasePage = FindPage(basePageName, childPage);

                if (childPage.BasePage == null)
                {
                    throw new Exception($"Cannot resolve '{basePageName}' base page for '{childPage.Name}' page.");
                }
            }
        }

        private void ResolveReferencingComponents()
        {
            foreach (var referencingComponent in _referencingComponents)
            {
                referencingComponent.Key.ReferencedComponent = FindComponent(referencingComponent.Value, referencingComponent.Key);

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

        private void ResolveInheritedComponents()
        {
            foreach (var baseComponent in _inheritedComponents)
            {
                baseComponent.Key.BaseComponent = FindComponent(baseComponent.Value, baseComponent.Key);

                if (baseComponent.Key.BaseComponent == null)
                {
                    throw new Exception($"Cannot resolve base '{baseComponent.Value}' component for '{baseComponent.Key.Name}'.");
                }

                // implicitly use By from base component
                if (baseComponent.Key.By == null)
                {
                    baseComponent.Key.By = baseComponent.Key.BaseComponent.By;
                }
            }
        }

        private ComponentContext FindComponent(string name, ComponentContext ignoredComponent)
        {
            return _flatComponents.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c != ignoredComponent);
        }

        private PageContext FindPage(string name, PageContext ignoredPage)
        {
            return _flatPages.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                && ignoredPage.Namespace.StartsWith(p.Namespace)
                && p != ignoredPage);
        }
    }
}
