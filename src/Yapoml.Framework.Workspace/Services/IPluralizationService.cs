using System;
using System.Collections.Generic;
using System.Text;

namespace Yapoml.Framework.Workspace.Services
{
    public interface IPluralizationService
    {
        bool IsPlural(string word);

        string Singularize(string word);
    }
}
