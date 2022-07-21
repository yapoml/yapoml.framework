using Humanizer;

namespace Yapoml.Framework.Workspace.Services
{
    public class PluralizationService : IPluralizationService
    {
        public string Singularize(string word)
        {
            return word.Singularize();
        }

        public bool IsPlural(string word)
        {
            return word.Pluralize() == word;
        }
    }
}

