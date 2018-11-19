using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class StringSearcher_Tests
    {
        private readonly StringSearcher searcher = new StringSearcher();
        private const string Input = "someinput";

        [Test]
        public void GetAllIndexesOfSubstrings_WorksOn_SingleSubstring()
        {
            var substrings = new HashSet<string>() {"in"};
            searcher.SplitBySubstrings(substrings, Input)
                .Should()
                .BeEquivalentTo(new List<Substring>()
                    {new Substring(0, "some"), new Substring(4, "in"), new Substring(6, "put")});
        }

        [Test]
        public void GetAllIndexesOfSubstrings_WorksOn_TwoSubstrings()
        {
            var substrings = new HashSet<string>() {"put", "in",};
            searcher.SplitBySubstrings(substrings, Input)
                .Should()
                .BeEquivalentTo(new List<Substring>()
                    {new Substring(0, "some"), new Substring(4, "in"), new Substring(6, "put")});
        }

        [Test]
        public void GetAllIndexesOfSubstrings_ReturnsEmpty_WhenEmptySubstrings()
        {
            var substrings = new HashSet<string>();
            searcher.SplitBySubstrings(substrings, Input).Should()
                .BeEquivalentTo(new List<Substring>() {new Substring(0, "someinput")});
        }
    }
}