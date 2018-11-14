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
        [TestCase(" ", " ")]
        [TestCase("\\\\", "\\")]
        [TestCase("_a_", "<em>a</em>")]
        [TestCase("__a__", "<strong>a</strong>")]
        [TestCase("\\_a_", "_a_")]
        [TestCase("\\__a__", "__a__")]
        [TestCase("_a\\_", "_a_")]
        [TestCase("__a\\__", "__a__")]
        [TestCase("a _b_", "a <em>b</em>")]
        [TestCase("a __b__", "a <strong>b</strong>")]
        [TestCase("a _b_ c", "a <em>b</em> c")]
        [TestCase("a __b__ c", "a <strong>b</strong> c")]
        [TestCase("_a_ b", "<em>a</em> b")]
        [TestCase("__a__ b", "<strong>a</strong> b")]
        [TestCase("a_b_ c", "a_b_ c")]
        [TestCase("a__b__ c", "a__b__ c")]
        [TestCase("a _b_c", "a _b_c")]
        [TestCase("a __b__c", "a __b__c")]
        [TestCase("a_b_c", "a_b_c")]
        [TestCase("a__b__c", "a__b__c")]
        [TestCase("__a _b_ c__", "<strong>a <em>b</em> c</strong>")]
        [TestCase("_a __b__ c_", "<em>a __b__ c</em>")]
        [TestCase("_a _b_ c_", "<em>a _b</em> c_")]
        [TestCase("__a __b__ c__", "<strong>a __b</strong> c__")]
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