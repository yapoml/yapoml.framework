using System.Text.RegularExpressions;

namespace Yapoml.Framework.Workspace.Services.Rules
{
    public class ReplaceRule
    {
        public Regex Condition { get; set; }
        public string ReplaceWith { get; set; }
    }
}
