using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MarkdownTests
    {
        [TestCase("a", "a")]
        [TestCase("_a_", "<em>a</em>")]
        [TestCase("__a__", "<strong>a</strong>")]
        public void TestRender(string inputString, string expectedResult)
        {
            var tags = new[]
            {
                new Tag("_", true, "_", "em"),
                new Tag("__", false, "__", "strong")
            };

            var allTags = tags.Select(tag => tag.OpeningTag).Concat(tags.Select(tag => tag.ClosingTag));
            var tagsTranslations = tags.ToDictionary(tag => tag.OpeningTag, tag => tag.Translation);
            var tagsInfo = tags.ToDictionary(tag => tag.OpeningTag, tag => tag.ToInfo());

            var parser = new MarkdownTokenParser(allTags);
            var tagTranslator = new MarkdownToHtmlTagTranslator(tagsTranslations);
            var treeTranslator = new MarkdownTokenTreeTranslator(tagTranslator);
            var treeBuilder = new MarkdownTokenTreeBuilder(tagsInfo);

            var markdown = new Markdown(parser, treeTranslator, treeBuilder);

            var translation = markdown.Render(inputString);

            translation.Should().Be(expectedResult);
        }
    }
}