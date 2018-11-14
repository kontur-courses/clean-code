using System.Linq;
using FluentAssertions;
using Markdown.Data;
using Markdown.Data.TagsInfo;
using Markdown.TokenParser;
using Markdown.TreeBuilder;
using Markdown.TreeTranslator;
using NUnit.Framework;

namespace MarkdownTests
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
                new Tag(new ItalicTagInfo(), "em"),
                new Tag(new BoldTagInfo(), "strong")
            };

            var allTags = tags.Select(tag => tag.Info.OpeningTag).Concat(tags.Select(tag => tag.Info.ClosingTag));
            var tagsTranslations = tags.ToDictionary(tag => tag.Info.OpeningTag, tag => tag.Translation);
            var tagsInfo = tags.Select(tag => tag.Info);

            var parser = new MarkdownTokenParser(allTags);
            var tagTranslator = new MarkdownToHtmlTagTranslator(tagsTranslations);
            var treeTranslator = new MarkdownTokenTreeTranslator(tagTranslator);
            var treeBuilder = new MarkdownTokenTreeBuilder(tagsInfo);

            var markdown = new Markdown.Markdown(parser, treeTranslator, treeBuilder);

            var translation = markdown.Render(inputString);

            translation.Should().Be(expectedResult);
        }
    }
}