using System.Collections.Generic;
using FluentAssertions;
using Markdown.Data;
using Markdown.Data.TagsInfo;
using Markdown.Data.TagsInfo.Headings;
using Markdown.Data.TagsInfo.StandardTags;
using Markdown.TreeBuilder;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTokenTreeBuilderTests
    {
        private IEnumerable<ITagInfo> tagsInfo;
        private MarkdownTokenTreeBuilder builder;
        private TestTreeBuilder testTreeBuilder;

        [SetUp]
        public void SetUp()
        {
            tagsInfo = new MarkdownLanguage().GetTagsInfo;
            builder = new MarkdownTokenTreeBuilder(tagsInfo);
            testTreeBuilder = TestTreeBuilder.Tree();
        }

        [Test]
        public void TestBuildTree_OnNoTokens()
        {
            var tokens = new Token[0];
            var expectedTree = testTreeBuilder
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnTextToken()
        {
            var tokens = new[] { new Token(TokenType.Text, "a") };
            var expectedTree = testTreeBuilder
                .WithText("a")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSpaceToken()
        {
            var tokens = new[] { new Token(TokenType.Space, " ") };
            var expectedTree = testTreeBuilder
                .WithSpace()
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnNewLineToken()
        {
            var tokens = new[] { new Token(TokenType.Tag, "\n") };
            var expectedTree = testTreeBuilder
                .WithText("\n")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnItalicTag()
        {
            var tokens = new[] { new Token(TokenType.Tag, "_") };
            var expectedTree = testTreeBuilder
                .WithText("_")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnBoldTag()
        {
            var tokens = new[] { new Token(TokenType.Tag, "__") };
            var expectedTree = testTreeBuilder
                .WithText("__")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapedEscapeSymbol()
        {
            var tokens = new[] { new Token(TokenType.Text, "\\") };
            var expectedTree = testTreeBuilder
                .WithText("\\")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnTextWithSpace()
        {
            var tokens = new[] 
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "b")
            };
            var expectedTree = testTreeBuilder
                .WithText("a")
                .WithSpace()
                .WithText("b")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnTextWithNewLine()
        {
            var tokens = new[]
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Text, "\n"),
                new Token(TokenType.Text, "b")
            };

            var expectedTree = testTreeBuilder
                .WithText("a")
                .WithText("\n")
                .WithText("b")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnItalicTagWithText()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "_")
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .Build())
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnHeading1WithText()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "# "),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "\n")
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new H1TagInfo())
                    .WithText("a")
                    .Build())
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnHeading2WithText()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "## "),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "\n")
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new H2TagInfo())
                    .WithText("a")
                    .Build())
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnHeading3WithText()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "### "),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "\n")
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new H3TagInfo())
                    .WithText("a")
                    .Build())
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnHeading4WithText()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "#### "),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "\n")
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new H4TagInfo())
                    .WithText("a")
                    .Build())
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnHeading5WithText()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "##### "),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "\n")
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new H5TagInfo())
                    .WithText("a")
                    .Build())
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnHeading6WithText()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "###### "),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "\n")
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new H6TagInfo())
                    .WithText("a")
                    .Build())
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnNotClosedItalicTag()
        {
            var tokens = new[] { new Token(TokenType.Tag, "_"), new Token(TokenType.Text, "a") };
            var expectedTree = testTreeBuilder
                .WithText("_")
                .WithText("a")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnNotOpenedItalicTag()
        {
            var tokens = new[] { new Token(TokenType.Text, "a"), new Token(TokenType.Tag, "_") };
            var expectedTree = testTreeBuilder
                .WithText("a")
                .WithText("_")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSpaceAfterOpeningItalicTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "_")
            };
            var expectedTree = testTreeBuilder
                .WithText("_")
                .WithSpace()
                .WithText("a")
                .WithText("_")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSpaceBeforeClosingItalicTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Tag, "_") 
            };
            var expectedTree = testTreeBuilder
                .WithText("_")
                .WithText("a")
                .WithSpace()
                .WithText("_")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnOpeningItalicTagInText()
        {
            var tokens = new[]
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "b"),
                new Token(TokenType.Tag, "_")
            };
            var expectedTree = testTreeBuilder
                .WithText("a")
                .WithText("_")
                .WithText("b")
                .WithText("_")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnClosingItalicTagInText()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "b")
            };
            var expectedTree = testTreeBuilder
                .WithText("_")
                .WithText("a")
                .WithText("_")
                .WithText("b")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapeBeforeOpeningItalicTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Text, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "_")
            };
            var expectedTree = testTreeBuilder
                .WithText("_")
                .WithText("a")
                .WithText("_")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapeBeforeClosingItalicTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Text, "_")
            };
            var expectedTree = testTreeBuilder
                .WithText("_")
                .WithText("a")
                .WithText("_")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapedOpeningSortTagInsideItalicTag()
        {
            var tokens = new[] 
                {
                    new Token(TokenType.Tag, "_"),
                    new Token(TokenType.Text, "a"),
                    new Token(TokenType.Space, " "),
                    new Token(TokenType.Text, "_"),
                    new Token(TokenType.Text, "b"),
                    new Token(TokenType.Tag, "_")
                };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithText("_")
                    .WithText("b")
                    .Build())
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapedClosingSortTagInsideItalicTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "b"),
                new Token(TokenType.Text, "_"),
                new Token(TokenType.Tag, "_")
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithText("b")
                    .WithText("_")
                    .Build())
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSameOpeningBeforeItalicTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "b"),
                new Token(TokenType.Tag, "_")
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithText("_")
                    .WithText("b")
                    .Build())
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSameClosingItalicTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "b"),
                new Token(TokenType.Tag, "_")
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .Build())
                .WithSpace()
                .WithText("b")
                .WithText("_")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnItalicTagInItalicTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "b"),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "c"),
                new Token(TokenType.Tag, "_"),
            };
            var expectedTree = testTreeBuilder
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

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnBoldTagInBoldTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Text, "b"),
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "c"),
                new Token(TokenType.Tag, "__"),
            };
            var expectedTree = testTreeBuilder
                .WithTag(TestTreeBuilder
                    .Tag(new ItalicTagInfo())
                    .WithText("a")
                    .WithSpace()
                    .WithText("__")
                    .WithText("b")
                    .Build())
                .WithSpace()
                .WithText("c")
                .WithText("__")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnBoldTagInsideItalicTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Text, "b"),
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "c"),
                new Token(TokenType.Tag, "_"),
            };
            var expectedTree = testTreeBuilder
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

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnItalicTagInsideBoldTag()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "b"),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "c"),
                new Token(TokenType.Tag, "__"),
            };
            var expectedTree = testTreeBuilder
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

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnOuterTagClosedBeforeInner()
        {
            var tokens = new[]
            {
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "b"),
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "c"),
                new Token(TokenType.Tag, "_"),
            };
            var expectedTree = testTreeBuilder
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

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }
    }
}