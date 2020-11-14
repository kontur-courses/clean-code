using System.Collections.Generic;
using System.Text;

namespace MarkdownParser.Helpers
{
    public static class StringHelpers
    {
        public static IEnumerable<string> SplitBy(this string source, char splitter)
        {
            var currentString = new StringBuilder();
            foreach (var symbol in source)
            {
                currentString.Append(symbol);
                if (symbol == splitter)
                {
                    yield return currentString.ToString();
                    currentString.Clear();
                }
            }

            if (currentString.Length != 0)
                yield return currentString.ToString();
        }
    }
}