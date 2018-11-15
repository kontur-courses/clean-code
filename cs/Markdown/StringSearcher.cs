using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Markdown
{
    class StringSearcher
    {
        public IEnumerable<Substring> GetAllSubstrings(HashSet<string> substrings, string stringToSearch)
        {
            if (substrings.Count == 0)
                return new List<Substring>();
            var result = new List<Substring>();

            var size = substrings.Max(x => x.Length);
            for (var i = 0; i < stringToSearch.Length; i++)
            {
                var current = "";
                for (var j = 0; j <= Math.Min(stringToSearch.Length - i, size); j++)
                {
                    if (substrings.Contains(stringToSearch.Substring(i, j)))
                    {
                        current = stringToSearch.Substring(i, j);
                    }
                }

                if (current.Length == 0)
                    continue;

                result.Add(new Substring(i, current));
                i += current.Length - 1;
            }

            return result;
        }

        public List<Substring> SplitBySubstrings(HashSet<string> substrings, string stringToSearch)
        {
            if (substrings.Count == 0)
                return new List<Substring> {new Substring(0, stringToSearch)};
            var result = new List<Substring>();

            var size = substrings.Max(x => x.Length);
            for (var i = 0; i < stringToSearch.Length; i++)
            {
                var current = "";
                for (var j = 0; j <= Math.Min(stringToSearch.Length - i, size); j++)
                {
                    if (substrings.Contains(stringToSearch.Substring(i, j)))
                    {
                        current = stringToSearch.Substring(i, j);
                    }
                }

                if (current.Length == 0)
                    continue;
                string text;
                int textIndex;
                if (result.Count == 0)
                {
                    text = stringToSearch.Substring(0, i);
                    textIndex = 0;
                }
                else
                {
                    textIndex = result.Last().Index + result.Last().Value.Length;
                    var length = i - textIndex;
                    text = stringToSearch.Substring(textIndex, length);
                }

                if (text != "")
                    result.Add(new Substring(textIndex, text));
                result.Add(new Substring(i, current));
                i += current.Length - 1;
            }

            if (stringToSearch.Substring(result.Last().Index + result.Last().Length) != "")
                result.Add(new Substring(result.Last().Index + result.Last().Length,
                    stringToSearch.Substring(result.Last().Index + result.Last().Length)
                ));
            return result;
        }
    }
}