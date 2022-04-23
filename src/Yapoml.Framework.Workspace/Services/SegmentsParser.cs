using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Yapoml.Framework.Workspace.Services
{
    internal class SegmentsParser
    {
        public IList<string> ParseSegments(string value)
        {
            // todo: performance, don't use regex for simple searching for {segment}
            var matches = Regex.Matches(value, "{(.*?)}");

            List<string> segments = null;

            if (matches.Count > 0)
            {
                segments = new List<string>();

                foreach (Match match in matches)
                {
                    segments.Add(match.Groups[1].Value);
                }
            }

            return segments;
        }
    }
}
