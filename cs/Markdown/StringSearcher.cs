using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class StringSearcher
    {
        public List<Substring> SplitBySubstrings(HashSet<string> substrings, string stringToSearch)
        {
            if (substrings.Count == 0)
            {
                return new List<Substring> {new Substring(0, stringToSearch)};
            }

            var result = new List<Substring>();
            var current = new StringBuilder();
            var width = substrings.Max(x => x.Length);
            for (var i = 0; i < stringToSearch.Length; i++)
            {
                if (!(substrings.Any(x => x.StartsWith(stringToSearch[i].ToString()))))
                {
                    current.Append(stringToSearch[i]);
                    continue;
                }

                if (current.Length > 0)
                {
                    result.Add(new Substring(i - current.Length, current.ToString()));
                    current = new StringBuilder();
                }

                var currentString = "";
                for (var j = 0; j <= Math.Min(stringToSearch.Length - i, width); j++)
                {
                    var currentPart = stringToSearch.Substring(i, j);
                    if (substrings.Contains(currentPart))
                        currentString = currentPart;
                }

                if (currentString != "")
                {
                    result.Add(new Substring(i, currentString));
                    i += currentString.Length - 1;
                }
            }

            if (current.Length > 0)
                result.Add(new Substring(stringToSearch.Length - current.Length, current.ToString()));
            return result;
        }
    }
}