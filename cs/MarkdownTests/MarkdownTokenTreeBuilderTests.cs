using System.Collections.Generic;
using FluentAssertions;
using Markdown.Data.TagsInfo;
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
            tagsInfo = new ITagInfo[] { new ItalicTagInfo(), new BoldTagInfo(), new H1TagInfo() };
            builder = new MarkdownTokenTreeBuilder(tagsInfo);
            testTreeBuilder = TestTreeBuilder.Tree();
        }

        [Test]
        public void TestBuildTree_OnNoTokens()
        {
            var tokens = new string[0];
            var expectedTree = testTreeBuilder
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnTextToken()
        {
            var tokens = new[] { "a" };
            var expectedTree = testTreeBuilder
                .WithText("a")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSpaceToken()
        {
            var tokens = new[] { " " };
            var expectedTree = testTreeBuilder
                .WithSpace()
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnNewLineToken()
        {
            var tokens = new[] { "\n" };
            var expectedTree = testTreeBuilder
                .WithText("\n")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnItalicTag()
        {
            var tokens = new[] { "_" };
            var expectedTree = testTreeBuilder.WithText("_").Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnBoldTag()
        {
            var tokens = new[] { "__" };
            var expectedTree = testTreeBuilder
                .WithText("__")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnDoubleEscapeSymbol()
        {
            var tokens = new[] { "\\", "\\" };
            var expectedTree = testTreeBuilder
                .WithText("\\")
                .Build();

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnTextWithSpace()
        {
            var tokens = new[] { "a", " ", "b" };
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
            var tokens = new[] { "a", "\n", "b" };
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
            var tokens = new[] { "_", "a", "_" };
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
        public void TestBuildTree_OnNotClosedItalicTag()
        {
            var tokens = new[] { "_", "a" };
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
            var tokens = new[] { "a", "_" };
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
            var tokens = new[] { "_", " ", "a", "_" };
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
            var tokens = new[] { "_", "a", " ", "_" };
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
            var tokens = new[] { "a", "_", "b", "_" };
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
            var tokens = new[] { "_", "a", "_", "b" };
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
            var tokens = new[] { "\\", "_", "a", "_" };
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
            var tokens = new[] { "_", "a", "\\", "_" };
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
            var tokens = new[] { "_", "a", " ", "\\", "_", "b", "_" };
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
            var tokens = new[] { "_", "a", " ", "b", "\\", "_", "_" };
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
            var tokens = new[] { "_", "a", " ", "_", "b", "_" };
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
            var tokens = new[] { "_", "a", "_", " ", "b", "_" };
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
            var tokens = new[] { "_", "a", " ", "_", "b", "_", " ", "c", "_" };
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
            var tokens = new[] { "__", "a", " ", "__", "b", "__", " ", "c", "__" };
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
            var tokens = new[] { "_", "a", " ", "__", "b", "__", " ", "c", "_" };
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
            var tokens = new[] { "__", "a", " ", "_", "b", "_", " ", "c", "__" };
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
            var tokens = new[] { "__", "a", " ", "_", "b", "__", " ", "c", "_" };
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