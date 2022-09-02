using Humanizer;

namespace Yapoml.Framework.Workspace.Services
{
    public class NameNormalizer : INameNormalizer
    {
        public string Normalize(string name)
        {
            return name
                .Replace("_", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ")
                .Pascalize()
                .Replace(" ", "");
        }
    }
}
