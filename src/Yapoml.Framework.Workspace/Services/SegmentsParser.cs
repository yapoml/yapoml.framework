using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Services
{
    internal static class SegmentsParser
    {
        public static IList<string> ParseSegments(string value)
        {
            List<string> segments = null;

            int currentOpeningPosition = -1;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '{')
                {
                    currentOpeningPosition = i;
                }
                else if (value[i] == '}' && currentOpeningPosition != -1)
                {
                    if (segments is null)
                    {
                        segments = new List<string>();
                    }

                    segments.Add(value.Substring(currentOpeningPosition + 1, i - currentOpeningPosition - 1));

                    currentOpeningPosition = -1;
                }
            }

            return segments;
        }
    }
}
