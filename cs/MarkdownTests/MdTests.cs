using System;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MdTests
    {
        [TestCaseSource(nameof(Render_SimpleTextWithOneTag_Cases))]
        public void Render_SimpleTextWithOneTag(string text, string expectedResult)
        {
            Md.Render(text).Should().Be(expectedResult);
        }

        private static object[] Render_SimpleTextWithOneTag_Cases =>
            new[]
            {
                new TestCaseData(
                        @"Текст, _окруженный с двух сторон_ ",
                        @"Текст, <em>окруженный с двух сторон</em> ")
                    .SetName("OnlyItalic"),

                new TestCaseData(
                        @"__Выделенный двумя символами текст__ должен становиться",
                        @"<strong>Выделенный двумя символами текст</strong> должен становиться")
                    .SetName("OnlyBold"),

                new TestCaseData(
                        @"# Спецификация языка разметки",
                        @"<h1> Спецификация языка разметки</h1>")
                    .SetName("OnlyHeader"),

                new TestCaseData(
                        @"* Пункт маркерованного списка",
                        @"<li> Пункт маркерованного списка</li>")
                    .SetName("OnlyUnorderedList")
            };

        [Test]
        public void Render_EscapeSymbolCanBeEscaped()
        {
            TestRender(
                @"можно экранировать: \\_вот это будет выделено тегом_",
                @"можно экранировать: \<em>вот это будет выделено тегом</em>");
        }

        [Test]
        public void Render_IfTagIsEscaped_InterpretItAsCommonCharacter()
        {
            TestRender(
                @"\_Вот это\_",
                @"_Вот это_");
        }

        [Test]
        public void Render_ParagraphStyleTagCanBeEscaped()
        {
            TestRender(
                @"\# ab cd.",
                @"# ab cd.");
        }

        [Test]
        public void Render_ItalicCanBeInBold()
        {
            TestRender(
                @"Внутри __двойного выделения _одинарное_ тоже__ работает.",
                @"Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.");
        }

        [Test]
        public void Render_BoldCannotBeInItalic()
        {
            TestRender(
                @"внутри _одинарного __двойное__ не_ работает.",
                @"внутри <em>одинарного __двойное__ не</em> работает.");
        }

        [Test]
        public void Render_TagsCannotContainsDigit()
        {
            TestRender(
                @"текста c цифрами_12_3 не считаются выделением",
                @"текста c цифрами_12_3 не считаются выделением");
        }

        [Test]
        public void Render_TagsCannotBeInsideDifferent()
        {
            TestRender(
                @"выделение в ра_зных сл_овах не работает",
                @"выделение в ра_зных сл_овах не работает");
        }

        [Test]
        public void Render_AfterOpenTagMustBeNotWhiteSpaceCharacter()
        {
            TestRender(
                @"эти_ подчерки_ не считаются выделением",
                @"эти_ подчерки_ не считаются выделением");
        }

        [Test]
        public void Render_BeforeClosingTagMustBeNotWhiteSpaceCharacter()
        {
            TestRender(
                @"_подчерки _не считаются_ окончанием",
                @"<em>подчерки _не считаются</em> окончанием");
        }

        [Test]
        public void Render_IfBoldAndItalicTagIntersect_IntepretItAsCommonCharacters()
        {
            TestRender(
                @"В случае __пересечения _двойных__ и одинарных_ подчерков",
                @"В случае __пересечения _двойных__ и одинарных_ подчерков");
        }

        [Test]
        public void Render_IfInsideTagsEmptyString_InterpretItAsCommonCharacter()
        {
            TestRender(
                @"Если внутри подчерков пустая строка ____, то они остаются символами подчерка.",
                @"Если внутри подчерков пустая строка ____, то они остаются символами подчерка.");
        }

        [Test]
        public void Render_AtHeaderMayBeAnotherTags()
        {
            TestRender(
                @"# Заголовок __с _разными_ символами__",
                @"<h1> Заголовок <strong>с <em>разными</em> символами</strong></h1>");
        }

        [Test]
        public void Render_IfHeaderTagIsNotAtStartOfLine_InterpretItAsCommonCharacter()
        {
            TestRender(
                @"Это # не заголовок",
                @"Это # не заголовок");
        }

        [Test]
        public void Render_AtUnorderedListMayBeAnotherTags()
        {
            TestRender(
                @"* Пункт __с _разными_ символами__",
                @"<li> Пункт <strong>с <em>разными</em> символами</strong></li>");
        }

        [Test]
        public void Render_IfUnorderedListTagIsNotAtStartOfLine_InterpretItAsCommonCharacter()
        {
            TestRender(
                @"Это * не пункт",
                @"Это * не пункт");
        }

        [Test]
        public void Render_WhenBoldTagDoesNotHaveClosingTag_InterpretItAsItalicTag()
        {
            TestRender(
                @"__Непарные_ символы",
                @"<em>_Непарные</em> символы");
        }

        [Test]
        public void Render_TestLinearComplexity()
        {
            var oneLine = @"# Заголовок __с _разными_ символами__; _ab _ac_;" +
                          Environment.NewLine +
                          "* _ab __cd__ cd_; __a _b c__ d_";

            var (smallText, largeText) = GetText(oneLine);

            var smallTime = CalculateTime(smallText);
            var largeTime = CalculateTime(largeText);

            var inputDataRatio = largeText.Length / smallText.Length;
            var timeRatio = largeTime / smallTime;

            Assume.That(timeRatio, Is.GreaterThanOrEqualTo(inputDataRatio - 2),
                message: "Complexity less than linear?");
            timeRatio.Should().BeInRange(inputDataRatio - 2, inputDataRatio + 2);
        }

        private static (string smallText, string largeText) GetText(string line)
        {
            var smallText = "";
            var largeText = "";
            var builder = new StringBuilder();
            for (int i = 1; i <= 1000; i++)
            {
                builder.Append(line);
                builder.Append(Environment.NewLine);
                if (i == 100)
                    smallText = builder.ToString();
                if (i == 1000)
                    largeText = builder.ToString();
            }

            return (smallText, largeText);
        }

        private static long CalculateTime(string text)
        {
            Md.Render(text);

            var timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < 100; i++)
            {
                Md.Render(text);
            }

            timer.Stop();
            return timer.ElapsedTicks / 100;
        }

        private static void TestRender(string textToRender, string expectedResult)
        {
            Md.Render(textToRender).Should().Be(expectedResult);
        }
    }
}