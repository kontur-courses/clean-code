using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Data;
using Markdown.Data.TagsInfo;
using Markdown.TreeTranslator;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTokenTreeTranslatorTests
    {
        private MarkdownTokenTreeTranslator treeTranslator;
        private MarkdownToHtmlTagTranslator tagTranslator;
        private IEnumerable<TagTranslationInfo> translations;
        private TestTreeBuilder treeBuilder;

        [SetUp]
        public void SetUp()
        {
            translations = new[] {new Tag(new ItalicTagInfo(), "em"), new Tag(new BoldTagInfo(), "strong")}
                    .Select(tag => tag.ToTranslationInfo);
            tagTranslator = new MarkdownToHtmlTagTranslator(translations);
            treeTranslator = new MarkdownTokenTreeTranslator(tagTranslator);
            treeBuilder = TestTreeBuilder.Tree();
        }

        [Test]
        public void TestBuildTree_OnTextToken()
        {
            const string expectedTranslation = "a";
            var tree = treeBuilder
                .WithText("a")
                .Build();
            
            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnSpaceToken()
        {
            const string expectedTranslation = " ";
            var tree = treeBuilder
                .WithSpace()
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnItalicTag()
        {
            const string expectedTranslation = "_";
            var tree = treeBuilder.WithText("_").Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnBoldTag()
        {
            const string expectedTranslation = "__";
            var tree = treeBuilder
                .WithText("__")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnDoubleEscapeSymbol()
        {
            const string expectedTranslation = @"\";
            var tree = treeBuilder
                .WithText("\\")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnItalicTagWithText()
        {
            const string expectedTranslation = "<em>a</em>";
            var tree = treeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .Build())
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnNotClosedItalicTag()
        {
            const string expectedTranslation = "_a";
            var tree = treeBuilder
                .WithText("_")
                .WithText("a")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnNotOpenedItalicTag()
        {
            const string expectedTranslation = "a_";
            var tree = treeBuilder
                .WithText("a")
                .WithText("_")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnSpaceAfterOpeningItalicTag()
        {
            const string expectedTranslation = "_ a_";
            var tree = treeBuilder
                .WithText("_")
                .WithSpace()
                .WithText("a")
                .WithText("_")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnSpaceBeforeClosingItalicTag()
        {
            const string expectedTranslation = "_a _";
            var tree = treeBuilder
                .WithText("_")
                .WithText("a")
                .WithSpace()
                .WithText("_")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnOpeningItalicTagInText()
        {
            const string expectedTranslation = "a_b_";
            var tree = treeBuilder
                .WithText("a")
                .WithText("_")
                .WithText("b")
                .WithText("_")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnClosingItalicTagInText()
        {
            const string expectedTranslation = "_a_b";
            var tree = treeBuilder
                .WithText("_")
                .WithText("a")
                .WithText("_")
                .WithText("b")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnEscapeBeforeOpeningItalicTag()
        {
            const string expectedTranslation = @"_a_";
            var tree = treeBuilder
                .WithText("_")
                .WithText("a")
                .WithText("_")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnEscapeBeforeClosingItalicTag()
        {
            const string expectedTranslation = @"_a_";
            var tree = treeBuilder
                .WithText("_")
                .WithText("a")
                .WithText("_")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnEscapedOpeningSortTagInsideItalicTag()
        {
            const string expectedTranslation = @"<em>a _b</em>";
            var tree = treeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithText("_")
                    .WithText("b")
                    .Build())
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnEscapedClosingSortTagInsideItalicTag()
        {
            const string expectedTranslation = @"<em>a b_</em>";
            var tree = treeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithText("b")
                    .WithText("_")
                    .Build())
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnSameOpeningBeforeItalicTag()
        {
            const string expectedTranslation = "<em>a _b</em>";
            var tree = treeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithText("_")
                    .WithText("b")
                    .Build())
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnSameClosingItalicTag()
        {
            const string expectedTranslation = "<em>a</em> b_";
            var tree = treeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .Build())
                .WithSpace()
                .WithText("b")
                .WithText("_")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnItalicTagInItalicTag()
        {
            const string expectedTranslation = "<em>a _b</em> c_";
            var tree = treeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithText("_")
                    .WithText("b")
                    .Build())
                .WithSpace()
                .WithText("c")
                .WithText("_")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnBoldTagInBoldTag()
        {
            const string expectedTranslation = "<strong>a __b</strong> c__";
            var tree = treeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new BoldTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithText("__")
                    .WithText("b")
                    .Build())
                .WithSpace()
                .WithText("c")
                .WithText("__")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnBoldTagInsideItalicTag()
        {
            const string expectedTranslation = "<em>a __b__ c</em>";
            var tree = treeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithTag(TestTreeBuilder
                        .RawTag(new BoldTagInfo())
                        .WithText("b")
                        .Build())
                    .WithSpace()
                    .WithText("c")
                    .Build())
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnItalicTagInsideBoldTag()
        {
            const string expectedTranslation = "<strong>a <em>b</em> c</strong>";
            var tree = treeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new BoldTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithTag(TestTreeBuilder
                        .Tag(new ItalicTagInfo())
                        .WithText("b")
                        .Build())
                    .WithSpace()
                    .WithText("c")
                    .Build())
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }

        [Test]
        public void TestBuildTree_OnOuterTagClosedBeforeInner()
        {
            const string expectedTranslation = "<strong>a _b</strong> c_";
            var tree = treeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new BoldTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithText("_")
                    .WithText("b")
                    .Build())
                .WithSpace()
                .WithText("c")
                .WithText("_")
                .Build();

            var translation = treeTranslator.Translate(tree);

            translation.Should().BeEquivalentTo(expectedTranslation);
        }
    }
}