using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest.ReducerTest
{
    public class ReducerTests
    {
        private Reducer reducer;
        
        [SetUp]
        public void SetUp()
        {
            reducer = new Reducer();
        }

        [TestCaseSource(typeof(ReducerSourceData), nameof(ReducerSourceData.Escape))]
        [TestCaseSource(typeof(ReducerSourceData), nameof(ReducerSourceData.Headers))]
        public void Simplify_ShouldWorkCorrectly_When(IEnumerable<IToken> tokens, IEnumerable<IToken> expected)
        {
            var actual = reducer.Reduce(tokens);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}