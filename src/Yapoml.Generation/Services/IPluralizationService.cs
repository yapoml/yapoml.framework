using System;
using System.Collections.Generic;
using System.Text;

namespace Yapoml.Generation.Services
{
    public interface IPluralizationService
    {
        bool IsPlural(string word);

        string Singularize(string word);
    }
}
