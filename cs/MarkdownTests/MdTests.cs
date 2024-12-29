using Markdown;
using FluentAssertions;

namespace MarkdownTests
{
    public class MdTests
    {
        public static IEnumerable<TestCaseData> NestedTagsTestCases
        {
            get
            {
                yield return new TestCaseData("Внутри __двойного выделения _одинарное_ тоже__ работает.",
                        "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.")
                    .SetName("WhenItalicTagInBold");
                yield return new TestCaseData("внутри _одинарного __двойное__ не_ работает.",
                        "внутри <em>одинарного __двойное__ не</em> работает.")
                    .SetName("NotRenderBold_WhenBoldInItalic");
                yield return new TestCaseData("# Заголовок __с _разными_ символами__",
                        "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")
                    .SetName("WhenManyTagsInHeader");
                yield return new TestCaseData("_этот текст весь будет\\_ выделен тегом_",
                        "<em>этот текст весь будет_ выделен тегом</em>")
                    .SetName("EscapeSymbol_WhenEscapeNestedInItalic");
                yield return new TestCaseData("__этот текст весь _будет\\__ выделен тегом__",
                        "<strong>этот текст весь <em>будет_</em> выделен тегом</strong>")
                    .SetName("EscapeSymbol_WhenEscapeNestedInItalicAndStrong");
                yield return new TestCaseData("В случае __пересечения _двойных__ и одинарных_ подчерков",
                        "В случае __пересечения _двойных__ и одинарных_ подчерков")
                    .SetName("NotRenderTag_WhenTheyHaveIntersection");
                yield return new TestCaseData("н_ачал_о",
                        "н<em>ачал</em>о")
                    .SetName("RenderTag_WhenSelectPartOfWordMiddle");
                yield return new TestCaseData("_на_чало",
                        "<em>на</em>чало")
                    .SetName("RenderTag_WhenSelectStartOfWord");
                yield return new TestCaseData("начал_о_",
                        "начал<em>о</em>")
                    .SetName("RenderTag_WhenSelectEndOfWord");
                yield return new TestCaseData("__FirstTag__, __SecondTag__",
                        "<strong>FirstTag</strong>, <strong>SecondTag</strong>")
                    .SetName("RenderTag_WhenFewDifferentTags1");
                yield return new TestCaseData("__AaAa__, _b_",
                        "<strong>AaAa</strong>, <em>b</em>")
                    .SetName("RenderTag_WhenFewDifferentTags2");
                yield return new TestCaseData("\\n",
                        "\n")
                    .SetName("LineEndSymbolNotEscapeTag");
                yield return new TestCaseData("__AaAa_b__",
                        "<strong>AaAa_b</strong>")
                    .SetName("NotRenderItalicTag_WhenUnpairItalicAndPairBold");
            }
        }
        public static IEnumerable<TestCaseData> IncorrectTagsTests
        {
            get
            {
                yield return new TestCaseData("__Непарные_ символы в рамках одного абзаца не считаются выделением.",
                        "_<em>Непарные</em> символы в рамках одного абзаца не считаются выделением.")
                    .SetName("UnpairSymbolsNotATags");
                yield return new TestCaseData("эти_ подчерки_ не считаются",
                        "эти_ подчерки_ не считаются")
                    .SetName("ShouldNotRenderTag_WhenUnderscoresHaveSpaceAfterItSelf");
                yield return new TestCaseData("не считается # ",
                        "не считается # ")
                    .SetName("ShouldNotRenderTag_WhenHeaderNotInStartParagraph");
                yield return new TestCaseData("ра_зных сл_овах ра__зных сл__овах не работает",
                        "ра_зных сл_овах ра__зных сл__овах не работает")
                    .SetName("ShouldNotRenderTag_WhenPairTagSelectPartOfDifferentWords");
                yield return new TestCaseData("_вот это будет \\д_ выделено тегом_",
                        "<em>вот это будет \\д</em> выделено тегом_")
                    .SetName("ShouldRenderTag_WhenEscapeWithOneSymbol");
                yield return new TestCaseData("____",
                       "____")
                   .SetName("ShouldNotRenderBoldTag_WhenInsideIsEmptyString");

            }
        }

        [TestCaseSource(nameof(NestedTagsTestCases))]
        [TestCaseSource(nameof(IncorrectTagsTests))]
        public void Render_Should(string mdText, string expectedTagsToHtml)
        {
            var md = new Md();

            var toHtml = md.Render(mdText);

            toHtml.Should().BeEquivalentTo(expectedTagsToHtml);
        }
    }
}
