using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class SimpleTests
    {
        [TestCase(
            "Текст, _окруженный с двух сторон_ одинарными символами подчерка.",
            "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка.",
            TestName = "Correctly Single Underlining")]

        [TestCase(
            "__Выделенный двумя символами текст__ должен становиться полужирным.",
            "<strong>Выделенный двумя символами текст</strong> должен становиться полужирным.",
            TestName = "Correctly Double Underlining")]
        public void UnderliningShouldHandled(string input, string expectedResult)
        {
            TestAssertions.AssertTest(input, expectedResult);
        }
    }

    [TestFixture]
    public class ShieldingTests
    {
        [TestCase(
            @"alp\\ha beta", @"alp\ha beta",
            TestName = "Should Shields Shield Sign")]

        [TestCase(
            @"_alpha\_ beta", @"_alpha_ beta",
            TestName = "Should Shields Underlining")]
        public void ShieldingShouldHandled(string input, string expectedResult)
        {
            TestAssertions.AssertTest(input, expectedResult);
        }
    }

    [TestFixture]
    public class HeaderTests
    {
        [TestCase(
            @"# Simple Header.", @"<h1>Simple Header.</h1>",
            TestName = "Simple Header Test")]

        [TestCase(
            @"# Заголовок __с _разными_ символами__", 
            @"<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>",
            TestName = "Header And Underlines Test")]
        public void HeadersShouldHandled(string input, string expectedResult)
        {
            TestAssertions.AssertTest(input, expectedResult);
        }
    }

    [TestFixture]
    public class TagCombinationsTests
    {
        [TestCase(
            @"Внутри __двойного выделения _одинарное_ тоже__ работает.",
            @"Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.",
            TestName = "Single Underlining Inside Double Underlining Test")]

        [TestCase(
            @"Но не наоборот — внутри _одинарного __двойное__ не_ работает.",
            @"Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.",
            TestName = "Double Underlining Inside Single Underlining Test")]

        [TestCase(
            @"_12_4_85_9",
            @"_12_4_85_9",
            TestName = "Underlining Between Digits Test")]

        [TestCase(
            "На улице бы_ла ясная__ погода. Мы решили прогуляться.",
            "На улице бы_ла ясная__ погода. Мы решили прогуляться.",
            TestName = "Unpaired Symbols In Paragraph Test")]

        [TestCase(
            "Эти_ подчерки_ не считаются выделением.",
            "Эти_ подчерки_ не считаются выделением.",
            TestName = "Should Be Not Space After Starting Underline Test")]

        [TestCase(
            "Эти _подчерки _не считаются выделением.",
            "Эти _подчерки _не считаются выделением.",
            TestName = "Should Be Not Space Before Ending Underline Test")]

        [TestCase("_нач_ало", "<em>нач</em>ало",
            TestName = "Underlining Beginning Of Word Test")]
        [TestCase("сер_ед_ина", "сер<em>ед</em>ина",
            TestName = "Underlining Middle Of Word Test")]
        [TestCase("кон_ец_", "кон<em>ец</em>",
            TestName = "Underlining Ending Of Word Test")]
        [TestCase("al__pha be__ta", "al__pha be__ta",
            TestName = "Double Underlining Inside Different Words Test")]
        [TestCase("al_pha be_ta", "al_pha be_ta",
            TestName = "Single Underlining Inside Different Words Test")]
        [TestCase("На __улице_ бы__ла ясная_ погода", "На __улице_ бы__ла ясная_ погода",
            TestName = "Intersection Between Double And Single Test")]
        [TestCase("На _улице__ бы_ла ясная__ погода", "На _улице__ бы_ла ясная__ погода",
            TestName = "Intersection Between Single And Double Test")]
        [TestCase("____", "____",
            TestName = "Empty Token Between Double Underlining Test")]
        [TestCase("__", "__",
            TestName = "Empty Token Between Single Underlining Test")]
        public void TagCombinationsShouldHandled(string input, string expectedResult)
        {
            TestAssertions.AssertTest(input, expectedResult);
        }
    }

    [TestFixture]
    public class LinkTests
    {
        [TestCase("[Эта ссылка](http://example.net/)",
            "<a href=\"http://example.net/\">Эта ссылка</a>",
            TestName = "SimpleLinkTest")]
        [TestCase("Empty _[To_ke_n](http://ex__ampl__e.net/) Between Double_ Underlining Test",
            "Empty <em><a href=\"http://ex__ampl__e.net/\">To_ke_n</a> Between Double</em> Underlining Test",
            TestName = "SimpleLinkTest!")]
        public void TagCombinationsShouldHandled(string input, string expectedResult)
        {
            TestAssertions.AssertTest(input, expectedResult);
        }
    }

    public class TestAssertions
    {
        public static void AssertTest(string input, string expected)
        {
            var markdown = new Md();
            markdown.Render(input).Should().Be(expected);
        }
    }
}
