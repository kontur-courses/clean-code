using Markdown;
using NUnit.Framework;
using FluentAssertions;

namespace MarkdownTests
{
    [TestFixture]
    public class MdSimpleTests
    {

        [Test]
        public void SimpleSingleUnderlyingShouldWorkCorrectly()
        {
            var input = "Текст, _окруженный с двух сторон_ одинарными символами подчерка.";
            var expectedResult = "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка.";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expectedResult);
            markdown.Render(input).Length.Should().Be(expectedResult.Length);
        }

        [Test]
        public void SimpleDoubleUnderlyingShouldWorkCorrectly()
        {
            var input = "__Выделенный двумя символами текст__ должен становиться полужирным.";
            var expectedResult = "<strong>Выделенный двумя символами текст</strong> должен становиться полужирным.";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expectedResult);
            markdown.Render(input).Length.Should().Be(expectedResult.Length);
        }
    }

    [TestFixture]
    public class MdShieldingTests
    {
        [Test]
        public void ShouldShieldeShieldSign()
        {
            var input = @"alp\\ha beta";
            var expectedResult = @"alp\ha beta";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expectedResult, expectedResult);
            markdown.Render(input).Length.Should().Be(expectedResult.Length);
        }

        [Test]
        public void ShouldShieldingUnderlying()
        {
            var input = @"_alpha\_ beta";
            var expectedResult = @"_alpha_ beta";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expectedResult);
            markdown.Render(input).Length.Should().Be(expectedResult.Length);
        }
    }

    [TestFixture]
    public class MdHeaderTests
    {
        [Test]
        public void HeaderTest()
        {
            var input = @"# Simple Header.";
            var expectedResult = @"<h1>Simple Header.</h1>";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expectedResult);
            markdown.Render(input).Length.Should().Be(expectedResult.Length);
        }

        [Test]
        public void HeaderAndUndelinesTest()
        {
            var input = @"# Заголовок __с _разными_ символами__";
            var expectedResult = @"<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expectedResult);
            markdown.Render(input).Length.Should().Be(expectedResult.Length);
        }
    }

    [TestFixture]
    public class MdTagCombinationsTests
    {
        [Test]
        public void DoubleUnderlyingInsideSingleUnderlying()
        {
            var input = @"Внутри __двойного выделения _одинарное_ тоже__ работает.";
            var expectedResult = @"Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expectedResult);
            markdown.Render(input).Length.Should().Be(expectedResult.Length);
        }

        [Test]
        public void SingleUnderlyingInsideDoubleUnderlying()
        {
            var input = @"Но не наоборот — внутри _одинарного __двойное__ не_ работает.";
            var expectedResult = @"Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expectedResult);
            markdown.Render(input).Length.Should().Be(expectedResult.Length);
        }

        [Test]
        public void UnderlyingBetweenDigitsShouldProcessedCorrectly()
        {

            var input = @"_12_4_85_9";
            var expectedResult = @"_12_4_85_9";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expectedResult);
            markdown.Render(input).Length.Should().Be(expectedResult.Length);
        }

        [TestCase("_нач_ало", "<em>нач</em>ало")]
        [TestCase("сер_ед_ина", "сер<em>ед</em>ина")]
        [TestCase("кон_ец_", "кон<em>ец</em>")]
        public void ShouldWorkCorrectInDifferentParts(string input, string expected)
        {
            var markdown = new Md();
            markdown.Render(input).Should().Be(expected, expected);
            //markdown.Render(input).Length.Should().Be(expected.Length);
        }

        [TestCase("unpa__ired sym__bols", "unpa__ired sym__bols")]
        [TestCase("al_pha be_ta", "al_pha be_ta")]
        public void UnderlineInsideDifferentWords(string input, string expected)
        {
            var markdown = new Md();
            markdown.Render(input).Should().Be(expected);
            markdown.Render(input).Length.Should().Be(expected.Length);
        }

        [Test]
        public void UnpairedSymbols()
        {
            var input = "На улице бы_ла ясная__ погода";
            var expected = "На улице бы_ла ясная__ погода";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expected);
            markdown.Render(input).Length.Should().Be(expected.Length);
        }

        [TestCase("На __улице_ бы__ла ясная_ погода")]
        [TestCase("На _улице__ бы_ла ясная__ погода")]
        public void IntersectingUnderlinesShouldProcessedCorrectly(string input)
        {
            var markdown = new Md();
            markdown.Render(input).Should().Be(input);
            markdown.Render(input).Length.Should().Be(input.Length);
        }

        [TestCase("____", "____")]
        [TestCase("__", "__")]
        public void ShouldAnalyzeEmptyStringsInsideTags(string input, string expected)
        {
            var markdown = new Md();
            markdown.Render(input).Should().Be(expected);
            markdown.Render(input).Length.Should().Be(expected.Length);
        }

        [Test]
        public void ShouldBeNotSpaceAfterStartingUnderline()
        {
            var input = "Эти_ подчерки_ не считаются выделением.";
            var expected = "Эти_ подчерки_ не считаются выделением.";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expected);
            markdown.Render(input).Length.Should().Be(expected.Length);
        }

        [Test]
        public void ShouldBeNotSpaceBeforeEndingUnderline()
        {
            //в примере после "считаются" стояло подчеркивание, мне кажется это опечатка, иначе противоречит описанию требования

            var input = "Эти _подчерки _не считаются выделением.";
            var expected = "Эти _подчерки _не считаются выделением.";
            var markdown = new Md();
            markdown.Render(input).Should().Be(expected);
            markdown.Render(input).Length.Should().Be(expected.Length);
        }
    }
}
