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
    class Substring
    {
        public int Index { get; private set; }
        public string Value { get; private set; }
        public int Length => Value.Length;

        public Substring(int index, string value)
        {
            Index = index;
            Value = value;
        }
    }

    class StringSearcher
    {
        public StringSearcher()
        {
        }

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

    [TestFixture]
    public class StringSearcher_Should
    {
        private StringSearcher searcher = new StringSearcher();

        [Test]
        public void GetAllIndexesOfSubstrings_WorksOn_SingleSubstring()
        {
            var input = "someinput";
            var substrings = new HashSet<string>() {"in"};
            searcher.GetAllSubstrings(substrings, input)
                .Should()
                .BeEquivalentTo(new List<Substring>() {new Substring(4, "in")});
        }

        [Test]
        public void GetAllIndexesOfSubstrings_WorksOn_TwoSubstrings()
        {
            var input = "someinput";
            var substrings = new HashSet<string>() {"put", "in",};
            searcher.GetAllSubstrings(substrings, input)
                .Should()
                .BeEquivalentTo(new List<Substring>() {new Substring(4, "in"), new Substring(6, "put")});
        }

        [Test]
        public void GetAllIndexesOfSubstrings_ReturnsEmpty_WhenEmptySubstrings()
        {
            var input = "someinput";
            var substrings = new HashSet<string>();
            searcher.GetAllSubstrings(substrings, input).Should().BeEmpty();
        }

        [Test]
        public void GetAllIndexesOfSubstrings_ReturnsEmpty_WhenNothingFound()
        {
            var input = "someinput";
            var substrings = new HashSet<string>() {"hi"};
            searcher.GetAllSubstrings(substrings, input).Should().BeEmpty();
        }
    }
}