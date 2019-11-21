using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Markdown.TokenConverters;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Markdown_Should
    {
        static Markdown markdown;
        static TokenConverter conventer;

        [SetUp]
        public void SetUp()
        {
            markdown = new Markdown();
            conventer = new HtmlConverter();
        }

        static IEnumerable GetLongCasesFromFiles()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "LargeTexts");
            foreach (var fileName in Directory.GetFiles(path))
                yield return File.ReadAllLines(Path.Combine(path, fileName));
        }

        [TestCase("_a_", "<em>a</em>", TestName = "Italic")]
        [TestCase("__a__", "<strong>a</strong>", TestName = "Bold")]
        [TestCase("__aa__ pfofoln_d_ddd_d__d__dkk___dadw_dwa___", 
            "<strong>aa</strong> pfofoln<em>d</em>ddd<em>d__d__dkk__</em>dadw<em>dwa__</em>", 
            TestName = "Stress test for very peaceful coders")]
        public void Render_InputCorrectString_ReturnHtmlFormatString(string input, string expected)
        {
            var actual = markdown.Render(input, conventer);
            actual.Should().Be(expected);
        }

        [TestCaseSource(nameof(GetLongCasesFromFiles))]
        public void Render_InputCorrectLargeStringFromFiles_ReturnHtmlFormatString(string[] input)
        {
            var expectedText = string
                .Concat(input
                    .Skip(input.Length / 2 + 1));

            var actualText = string.Concat(
                input.Take(input.Length / 2)
                    .Select(item => markdown.Render(item, conventer)));
            
            actualText.Should().Be(expectedText);
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
            var actual = markdown.Render(input, conventer);
            actual.Should().Be(input);
        }

        [TestCase(@"\_a\_", @"_a_", TestName = "Two shielded between symbol")]
        [TestCase(@"\_a", @"_a",  TestName = "Shielded before symbol")]
        [TestCase(@"_\a", @"_\a", TestName = "Shielded not key symbol")]
        [TestCase(@"\\a", @"\a", TestName = "Shielded itself")]
        [TestCase(@"\__a", @"__a", TestName = "Shielded underline in two underlines")]
        public void Render_InputShieldedString_RemoveShieldsAndReturnWithoutHtmlFormation(string input, string expected)
        {
            var actual = markdown.Render(input, conventer);
            actual.Should().Be(expected);
        }
    }
}