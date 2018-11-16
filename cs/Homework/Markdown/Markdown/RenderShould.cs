using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class RenderShould
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
        [TestCase("\\_a\\_", "_a_", TestName = "With escaping")]
        // Продолжить тесты здесь
        public void ReturnTheSameString(string mdParagraph, string expected)
        {
            var actual = md.Render(mdParagraph);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
