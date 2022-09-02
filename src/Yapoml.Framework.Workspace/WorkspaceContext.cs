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
        private readonly INameNormalizer _nameNormalizer;

        public WorkspaceContext(string rootDirectoryPath, string rootNamespace, IWorkspaceParser parser, IWorkspaceReferenceResolver workspaceWalker, INameNormalizer nameNormalizer)
        {
            RootDirectoryPath = rootDirectoryPath.Replace("/", "\\").TrimEnd('\\');
            RootNamespace = rootNamespace;
            _parser = parser;
            _workspaceReferenceResolver = workspaceWalker;
            _nameNormalizer = nameNormalizer;
        }

        public string RootDirectoryPath { get; }

        public string RootNamespace { get; }

        public IList<SpaceContext> Spaces { get; } = new List<SpaceContext>();

        public IList<PageContext> Pages { get; } = new List<PageContext>();

        public IList<ComponentContext> Components { get; } = new List<ComponentContext>();

        public void AddFile(string filePath, string content)
        {
            var space = CreateOrAddSpaces(filePath);

            if (filePath.ToLowerInvariant().EndsWith(".po.yaml"))
            {
                var pages = _parser.ParsePages(content);

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
                        pageContext = new PageContext(pageName, this, null, page, _nameNormalizer);

                        Pages.Add(pageContext);
                    }
                    else
                    {
                        pageContext = new PageContext(pageName, this, space, page, _nameNormalizer);

                        space.Pages.Add(pageContext);
                    }

                    _workspaceReferenceResolver.AppendPage(pageContext);
                }
            }
            else if (filePath.ToLowerInvariant().EndsWith(".pc.yaml"))
            {
                var component = _parser.ParseComponent(content);

                var fileName = Path.GetFileName(filePath);

                if (string.IsNullOrEmpty(component.Name))
                {
                    component.Name = fileName.Substring(0, fileName.Length - ".pc.yaml".Length);
                }

                ComponentContext componentContext;

                if (space == null)
                {
                    componentContext = new ComponentContext(this, null, null, null, component, _nameNormalizer);

                    Components.Add(componentContext);
                }
                else
                {
                    componentContext = new ComponentContext(this, space, null, null, component, _nameNormalizer);

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

        private SpaceContext CreateOrAddSpaces(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);

            var path = directory.Substring(RootDirectoryPath.Length);

            var parts = path.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 0)
            {
                SpaceContext nestedSpace = Spaces.FirstOrDefault(s => s.Namespace == $"{RootNamespace}.{_nameNormalizer.Normalize(parts[0])}");

                if (nestedSpace == null)
                {
                    nestedSpace = new SpaceContext(parts[0], this, null, _nameNormalizer);

                    Spaces.Add(nestedSpace);
                }

                for (int i = 1; i < parts.Length; i++)
                {
                    var candidateNestedSpace = nestedSpace.Spaces.FirstOrDefault(s => s.Name == _nameNormalizer.Normalize(parts[i]));

                    if (candidateNestedSpace == null)
                    {
                        var newNestedSpace = new SpaceContext(parts[i], this, nestedSpace, _nameNormalizer);

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
