using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Markdown_Should
    {
        static Markdown markdown;

        [SetUp]
        public void SetUp() => markdown = new Markdown();

        [TestCase("_a_", "<em>a</em>")]
        [TestCase("__a__", "<strong>a</strong>")]
        [TestCase("__aa__ pfofoln_d_ddd_d__d__dkk___dadw_dwa___", 
            "<strong>aa</strong> pfofoln<em>d</em>ddd<em>d__d__dkk__</em>dadw<em>dwa__</em>", TestName = "Stress test for very peaceful coders")]
        public void Render_InputCorrectString_ReturnHtmlFormatString(string input, string expected)
        {
            var actual = markdown.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase("_a", TestName = "Underline before symbol")]
        [TestCase("a_", TestName = "Underline after symbol")]
        [TestCase("__a", TestName = "Two underlines before symbol")]
        [TestCase("a__", TestName = "Two underlines after symbol")]
        [TestCase("цифрами_12_3", TestName = "Underline between digits")]
        [TestCase("this_ underlines_ not read(", TestName = "Two end tags")]
        [TestCase("this _underlines _not read(", TestName = "Two start tags")]
        public void Render_InputIncorrectString_ReturnSameString(string input)
        {
            var actual = markdown.Render(input);
            actual.Should().Be(input);
        }

        [TestCase(@"\_a\_", @"_a_", TestName = "Two shielded between symbol")]
        [TestCase(@"\_a", @"_a",  TestName = "Shielded before symbol")]
        [TestCase(@"_\a", @"_\a", TestName = "Shielded not key symbol")]
        [TestCase(@"\\a", @"\a", TestName = "Shielded itself")]
        [TestCase(@"\__a", @"__a", TestName = "Shielded underline in two underlines")]
        public void Render_InputShieldedString_RemoveShieldsAndReturnWithoutHtmlFormation(string input, string expected)
        {
            var actual = markdown.Render(input);
            actual.Should().Be(expected);
        }
    }
}