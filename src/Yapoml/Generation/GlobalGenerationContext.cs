using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yapoml.Parsers;

namespace Yapoml.Generation
{
    public class GlobalGenerationContext
    {
        public GlobalGenerationContext(string rootDirectoryPath, string rootNamespace, IParser parser)
        {
            RootDirectoryPath = rootDirectoryPath.Replace("/", "\\").TrimEnd('\\');
            RootNamespace = rootNamespace;
            Parser = parser;
        }

        private IParser Parser { get; }

        public string RootDirectoryPath { get; }

        public string RootNamespace { get; }

        public IList<SpaceGenerationContext> Spaces { get; } = new List<SpaceGenerationContext>();

        public IList<PageGenerationContext> Pages { get; } = new List<PageGenerationContext>();

        public IList<ComponentGenerationContext> Components { get; } = new List<ComponentGenerationContext>();

        public void AddFile(string filePath)
        {
            var space = CreateOrAddSpaces(filePath);

            if (filePath.ToLowerInvariant().EndsWith(".po.yaml"))
            {
                var page = Parser.ParsePage(filePath);
                var fileName = Path.GetFileName(filePath);
                var pageName = fileName.Substring(0, fileName.Length - 8);

                if (space == null)
                {
                    var pageContext = new PageGenerationContext(pageName, this, null, page);

                    Pages.Add(pageContext);
                }
                else
                {
                    var pageContext = new PageGenerationContext(pageName, this, space, page);

                    space.Pages.Add(pageContext);
                }
            }
            else if (filePath.ToLowerInvariant().EndsWith(".pc.yaml"))
            {
                var component = Parser.ParseComponent(filePath);

                var fileName = Path.GetFileName(filePath);

                var componentName = fileName.Substring(0, fileName.Length - 8);

                if (space == null)
                {
                    var componentContext = new ComponentGenerationContext(componentName, this, null, component);

                    Components.Add(componentContext);
                }
                else
                {
                    var componentContext = new ComponentGenerationContext(componentName, this, space, component);

                    space.Components.Add(componentContext);
                }
            }
        }

        private SpaceGenerationContext CreateOrAddSpaces(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);

            var path = directory.Substring(RootDirectoryPath.Length);

            var parts = path.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 0)
            {
                SpaceGenerationContext nestedSpace = Spaces.FirstOrDefault(s => s.Namespace == $"{RootNamespace}.{parts[0]}");

                if (nestedSpace == null)
                {
                    nestedSpace = new SpaceGenerationContext(parts[0], this, null);

                    Spaces.Add(nestedSpace);
                }

                for (int i = 1; i < parts.Length; i++)
                {
                    var candidateNestedSpace = nestedSpace.Spaces.FirstOrDefault(s => s.Name == parts[i]);

                    if (candidateNestedSpace == null)
                    {
                        var newNestedSpace = new SpaceGenerationContext(parts[i], this, nestedSpace);

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
