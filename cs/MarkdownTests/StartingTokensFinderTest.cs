using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    class StartingTokensFinderTest
    {
        [TestCase("# word", new[] { 0 }, TestName = "Should find simple lattice")]
        public void FindDoubleAndSimple(string paragraph, int[] positions)
        {
            var finder = new StartingTokenFinder();

            var tokens = finder.FindStartingTokens(paragraph);
            tokens.First().TokenPosition.ShouldBeEquivalentTo(positions.First());
        }
    }
}
