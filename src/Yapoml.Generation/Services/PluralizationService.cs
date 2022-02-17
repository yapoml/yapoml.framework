using System;
using System.Collections.Generic;
using System.Text;

namespace Yapoml.Generation.Services
{
    internal class PluralizationService
    {
        // dummy
        public bool IsPlural(string word)
        {
            if (word.EndsWith("s") || word.EndsWith("(s)"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
