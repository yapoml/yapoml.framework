using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yapoml.Framework.Workspace.Parsers;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Workspace
{
    public class WorkspaceContext
    {
        private readonly IWorkspaceParser _parser;
        private readonly IWorkspaceReferenceResolver _workspaceReferenceResolver;

        public WorkspaceContext(string rootDirectoryPath, string rootNamespace, IWorkspaceParser parser, IWorkspaceReferenceResolver workspaceWalker, INameNormalizer nameNormalizer)
        {
            RootDirectoryPath = rootDirectoryPath.Replace("/", "\\").TrimEnd('\\');
            RootNamespace = rootNamespace;
            _parser = parser;
            _workspaceReferenceResolver = workspaceWalker;
            NameNormalizer = nameNormalizer;
        }

        public string RootDirectoryPath { get; }

        public string RootNamespace { get; }
        public INameNormalizer NameNormalizer { get; }
        public IList<SpaceContext> Spaces { get; } = new List<SpaceContext>();

        public IList<PageContext> Pages { get; } = new List<PageContext>();

        public IList<ComponentContext> Components { get; } = new List<ComponentContext>();

        public void AddFile(string filePath, string content)
        {
            if (TryGetPageOrComponentFile(filePath, out var pageName))
            {
                var space = CreateOrAddSpaces(filePath);

                var pages = _parser.ParsePages(content);

                for (int i = 0; i < pages.Count; i++)
                {
                    var page = pages[i];

                    // adjust page family
                    if (i != 0)
                    {
                        pageName = $"{pageName}_{i}";
                    }

                    page.Name = pageName;

                    PageContext pageContext;

                    if (space == null)
                    {
                        pageContext = new PageContext(this, null, page, GetRelativeFilePath(filePath));

                        Pages.Add(pageContext);
                    }
                    else
                    {
                        pageContext = new PageContext(this, space, page, GetRelativeFilePath(filePath));

                        space.Pages.Add(pageContext);
                    }

                    _workspaceReferenceResolver.AppendPage(pageContext);
                }
            }
            else if (TryGetComponentFile(filePath, out var componentName))
            {
                var space = CreateOrAddSpaces(filePath);

                var component = _parser.ParseComponent(content);

                if (string.IsNullOrEmpty(component.Name))
                {
                    component.Name = componentName;
                }

                ComponentContext componentContext;

                if (space == null)
                {
                    componentContext = new ComponentContext(this, null, null, null, component, GetRelativeFilePath(filePath));

                    Components.Add(componentContext);
                }
                else
                {
                    componentContext = new ComponentContext(this, space, null, null, component, GetRelativeFilePath(filePath));

                    space.Components.Add(componentContext);
                }

                _workspaceReferenceResolver.AppendComponent(componentContext);
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
            _workspaceReferenceResolver.Resolve();
        }

        private bool TryGetPageOrComponentFile(string filePath, out string pageName)
        {
            var fileName = Path.GetFileName(filePath);

            if (filePath.EndsWith(".page.yml", StringComparison.InvariantCultureIgnoreCase))
            {
                pageName = fileName.Substring(0, fileName.Length - ".page.yml".Length);
                return true;
            }
            else if (filePath.EndsWith(".page.yaml", StringComparison.InvariantCultureIgnoreCase))
            {
                pageName =  fileName.Substring(0, fileName.Length - ".page.yaml".Length);
                return true;
            }
            else
            {
                pageName = null;
                return false;
            }
        }

        private bool TryGetComponentFile(string filePath, out string componentName)
        {
            var fileName = Path.GetFileName(filePath);

            if (filePath.EndsWith(".component.yml", StringComparison.InvariantCultureIgnoreCase))
            {
                componentName = fileName.Substring(0, fileName.Length - ".component.yml".Length);
                return true;
            }
            else if (filePath.EndsWith(".component.yaml", StringComparison.InvariantCultureIgnoreCase))
            {
                componentName = fileName.Substring(0, fileName.Length - ".component.yaml".Length);
                return true;
            }
            else
            {
                componentName = null;
                return false;
            }
        }

        private SpaceContext CreateOrAddSpaces(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);

            var path = directory.Substring(RootDirectoryPath.Length);

            var parts = path.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 0)
            {
                var normalizedSpaceName = NameNormalizer.Normalize(parts[0]);

                SpaceContext nestedSpace = Spaces.FirstOrDefault(s => s.Namespace == $"{RootNamespace}.{normalizedSpaceName}");

                if (nestedSpace == null)
                {
                    nestedSpace = new SpaceContext(normalizedSpaceName, this, null);

                    Spaces.Add(nestedSpace);
                }

                for (int i = 1; i < parts.Length; i++)
                {
                    normalizedSpaceName = NameNormalizer.Normalize(parts[i]);

                    var candidateNestedSpace = nestedSpace.Spaces.FirstOrDefault(s => s.Name == normalizedSpaceName);

                    if (candidateNestedSpace == null)
                    {
                        var newNestedSpace = new SpaceContext(normalizedSpaceName, this, nestedSpace);

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

        private string GetRelativeFilePath(string fullFilePath)
        {
            return fullFilePath.Substring(RootDirectoryPath.Length + 1);
        }
    }
}
