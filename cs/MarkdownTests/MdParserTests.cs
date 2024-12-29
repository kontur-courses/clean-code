using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    public class MdParserTests
    {
        public static IEnumerable<TestCaseData> DifferentTagsParseWithoutNestedAndIntersection
        {
            get
            {
                yield return new TestCaseData("_окруженный_ с двух __сторон__",
                        new []
                        {
                            "<em>окруженный</em>", "<strong>сторон</strong>"
                        })
                    .SetName("WhenHasTwoTagsWithoutIntersection");
                yield return new TestCaseData("\\_Вот это\\_",
                        new[]
                        {
                            "_", "_"
                        })
                    .SetName("WhenEscapeTagEscapeItalicTagStartAndEnd");
                yield return new TestCaseData("\\\\_вот это будет выделено тегом_",
                        new[]
                        {
                            "\\", "<em>вот это будет выделено тегом</em>"
                        })
                    .SetName("WhenEscapeTagEscapeItself");
            }
        }
        [TestCase("# Hello World!\nHi, Mari!", "<h1>Hello World!</h1>", TestName = "HeaderTag")]
        [TestCase("dsf\\_sdf", "_", TestName = "EscapeTag")]
        [TestCase("__Выделенный двумя символами текст__", "<strong>Выделенный двумя символами текст</strong>", TestName = "BoldTag")]
        [TestCase("_окруженный с двух сторон_", "<em>окруженный с двух сторон</em>", TestName = "ItalicTag")]
        [TestCase("# Hello World!", "<h1>Hello World!</h1>", TestName = "HeaderTagWithoutParagraphEndSymbol")]
        public void MdParser_ShouldCorrectParse(string mdText, string expected)
        {
            var parser = new MdParser(mdText, Md.MdTags);

            var tags = parser.GetTags();
            var toHtml = tags[0].RenderToHtml();

            toHtml.Should().Be(expected);
        }

        [TestCaseSource(nameof(DifferentTagsParseWithoutNestedAndIntersection))]
        public void MdParser_ShouldCorrectParseDifferentTags(string mdText, string[] expectedTagsToHtml)
        {
            var parser = new MdParser(mdText, Md.MdTags);

            var tags = parser.GetTags().Select(t => t.RenderToHtml());

            tags.Should().BeEquivalentTo(expectedTagsToHtml);
        }

    }
}