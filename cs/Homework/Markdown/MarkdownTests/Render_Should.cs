using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class Render_Should
    {
        private readonly Md md = new Md();

        [TestCase("aaa", "aaa", TestName = "No tags")]
        [TestCase("a __a__ a", "a <strong>a</strong> a", TestName = "Valid strong")]
        [TestCase("a _a_ a", "a <em>a</em> a", TestName = "Valid emphasis")]
        [TestCase("a __a__ _a_", "a <strong>a</strong> <em>a</em>", TestName = "Valid strong and emphasis")]
        [TestCase("a _a_.", "a <em>a</em>.", TestName = "Valid with punctuation after")]
        [TestCase("_a_", "<em>a</em>", TestName = "Valid with starting the line")]
        [TestCase("_a __a__ a_", "<em>a __a__ a</em>", TestName = "Strong in emphasis")]
        [TestCase("__a _a_ a__", "<strong>a <em>a</em> a</strong>", TestName = "Emphasis in strong")]
        [TestCase(@"\_a\_", "_a_", TestName = "With escaping")]
        [TestCase(@"\\\_a\\\_", "_a_", TestName = "With odd escaping count")]
        [TestCase(@"\\_a_", "<em>a</em>", TestName = "With even escaping count")]
        [TestCase("# _hello_\r", "<h1> <em>hello</em>", TestName = "Header not break nesting tags")]
        [TestCase("# _hello_", "# <em>hello</em>", TestName = @"Without \r header not valid")]
        public void ReturnTheSameString(string mdLine, string expected)
        {
            var actual = md.Render(mdLine);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
