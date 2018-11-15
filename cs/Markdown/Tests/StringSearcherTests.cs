using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class StringSearcher_Tests
    {
        private StringSearcher searcher = new StringSearcher();

        [Test]
        public void GetAllIndexesOfSubstrings_WorksOn_SingleSubstring()
        {
            var input = "someinput";
            var substrings = new HashSet<string>() { "in" };
            searcher.GetAllSubstrings(substrings, input)
                .Should()
                .BeEquivalentTo(new List<Substring>() { new Substring(4, "in") });
        }

        [Test]
        public void GetAllIndexesOfSubstrings_WorksOn_TwoSubstrings()
        {
            var input = "someinput";
            var substrings = new HashSet<string>() { "put", "in", };
            searcher.GetAllSubstrings(substrings, input)
                .Should()
                .BeEquivalentTo(new List<Substring>() { new Substring(4, "in"), new Substring(6, "put") });
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
            var substrings = new HashSet<string>() { "hi" };
            searcher.GetAllSubstrings(substrings, input).Should().BeEmpty();
        }
    }
}
