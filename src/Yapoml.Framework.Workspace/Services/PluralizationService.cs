using Humanizer;

namespace Yapoml.Framework.Workspace.Services
{
    public class PluralizationService : IPluralizationService
    {
        public string Singularize(string word)
        {
            var singularWord = word;

            if (IsPlural(word))
            {
                singularWord = word.Singularize();
            }

            return singularWord;
        }

        public bool IsPlural(string word)
        {
            return word.Pluralize() == word;
        }
    }
}

