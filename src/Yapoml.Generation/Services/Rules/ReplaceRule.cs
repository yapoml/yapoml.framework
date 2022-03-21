using System.Text.RegularExpressions;

namespace Yapoml.Generation.Services.Rules
{
    public class ReplaceRule
    {
        public Regex Condition { get; set; }
        public string ReplaceWith { get; set; }
    }
}
