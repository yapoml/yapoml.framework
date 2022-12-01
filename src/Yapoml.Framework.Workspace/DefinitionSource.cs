using Yapoml.Framework.Workspace.Parsers;

namespace Yapoml.Framework.Workspace
{
    public class DefinitionSource
    {
        public DefinitionSource(string relativeFilePath, Region region)
        {
            RelativeFilePath = relativeFilePath;
            Region = region;
        }

        public string RelativeFilePath { get; }

        public Region Region { get; }
    }
}
