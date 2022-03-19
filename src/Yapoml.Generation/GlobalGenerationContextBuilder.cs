using System.Collections.Generic;
using Yapoml.Generation.Parsers;

namespace Yapoml.Generation
{
    public class GlobalGenerationContextBuilder
    {
        private readonly string _rootDirectoryPath;
        private readonly string _rootNamespace;
        private readonly IParser _parser;

        private readonly IList<string> _files = new List<string>();

        public GlobalGenerationContextBuilder(string rootDirectoryPath, string rootNamespace, IParser parser)
        {
            _rootDirectoryPath = rootDirectoryPath;
            _rootNamespace = rootNamespace;
            _parser = parser;
        }

        public GlobalGenerationContextBuilder AddFile(string filePath)
        {
            _files.Add(filePath);

            return this;
        }

        public GlobalGenerationContext Build()
        {
            var globalContext = new GlobalGenerationContext(_rootDirectoryPath, _rootNamespace, _parser);

            foreach (var filePath in _files)
            {
                globalContext.AddFile(filePath);
            }

            globalContext.ResolveReferences();

            return globalContext;
        }
    }
}
