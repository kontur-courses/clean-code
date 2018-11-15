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
    }
}