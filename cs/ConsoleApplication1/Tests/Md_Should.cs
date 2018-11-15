using System;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace ConsoleApplication1.Tests
{
    [TestFixture]
    public class Md_Should
    {
        private readonly Md md = new Md();

        [TestCase("aaaaaaa", "aaaaaaa", TestName = "no underscores")]
        [TestCase("__a a__", "<strong>a a</strong>", TestName = "one bold tag")]
        [TestCase("___a a___", "<strong><em>a a</em></strong>", TestName = "one italic and bold tag")]
        [TestCase("____a a____", "<strong><em>a a</em></strong>", TestName = "one odd underscore")]
        [TestCase(@"\a", "a", TestName = "escape character")]
        [TestCase("_a _a a_ a_", "<em>a a a a</em>", TestName = "remove meaningless selections")]
        [TestCase("__a __a a__ a__", "<strong>a <em>a a</em> a</strong>", TestName = "transform bold into italic, when it is nested in bold")]
        [TestCase(@"\_ _", "_ _", TestName = "do not connect escapes underscore")]
        [TestCase("_a __b c__ d_", "<em>a b c d</em>", TestName = "do not make bold selection inside italic")]
        [TestCase(" _ b_", " _ b_", TestName = "do not count underscore without text behind or after")]
        [TestCase("a_c  a_", "a_c  a_", TestName = "do not count underscore inside of text")]
        [TestCase(@"_\_ a_", "<em>_ a</em>", TestName = "count escaped underscore as text")]
        [TestCase("_a a_ _a a_", "<em>a a</em> <em>a a</em>", TestName = "do not remove repeating selections, when there are not nested selections")]
        [TestCase("_a a_ b_", "<em>a a</em> b_", TestName = "connect the closest underscores")]
        [TestCase("___a a_ b__", "<strong><em>a a</em> b</strong>", TestName = "underscores can connect with different underscores")]
        [TestCase(@"\ _a b_", " <em>a b</em>", TestName = "count escaped white-spaces as usual spaces")]
        public void Render_ReturnsCorrectConversion_WhenGetsCurrentString(string givenString, string expectedString)
        {
            md.Render(givenString)
                .Should()
                .Be(expectedString);
        }

        [Test, Timeout(1000)]
        public void Render_WorksFast_WhenThereAreTooBigString()
        {
            var countRepetitions = 25000;
            var renderedString = new StringBuilder();
            var addedRepetitions = new[] { "___a", "__a", "_a", "a__", "a___", "a_" };
            foreach (var repeatedText in addedRepetitions)
            {
                for (var index = 0; index < countRepetitions; index++)
                    renderedString.Append(repeatedText);
            }

            md.Render(renderedString.ToString());
        }
    }
}