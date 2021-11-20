﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Yapoml.Parsers.Yaml;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Generation
{
    public class GlobalGenerationContext
    {
        public GlobalGenerationContext(string rootDirectoryPath, string rootNamespace)
        {
            RootDirectoryPath = rootDirectoryPath.Replace("/", "\\");
            RootNamespace = rootNamespace;
        }

        public string RootDirectoryPath { get; }
        public string RootNamespace { get; }

        public IList<SpaceGenerationContext> Spaces { get; } = new List<SpaceGenerationContext>();

        public void AddFile(string filePath)
        {
            CreateOrAddSpaces(filePath);

            //if (space != null)
            //{
            // add file into space
            //}
            //else
            //{
            // add file in global context
            //}

            //var component = new YamlParser().Parse<Component>(File.ReadAllText(filePath));


        }

        private void CreateOrAddSpaces(string filePath)
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
            }
        }
    }
}