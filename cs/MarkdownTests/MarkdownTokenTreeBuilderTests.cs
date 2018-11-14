using System.Collections.Generic;
using FluentAssertions;
using Markdown.Data.Nodes;
using Markdown.Data.TagsInfo;
using Markdown.TreeBuilder;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTokenTreeBuilderTests
    {
        // Ну здесь скорее всего нужен какой-то DataBuilder для деревьев
        // Он еще потом пригодится в тестах на Translator, чтобы генерировать входные данные

        private IEnumerable<ITagInfo> tagsInfo;
        private MarkdownTokenTreeBuilder builder;
        private TagTreeNode expectedTree;

        [SetUp]
        public void SetUp()
        {
            tagsInfo = new ITagInfo[] { new ItalicTagInfo(), new BoldTagInfo() };
            builder = new MarkdownTokenTreeBuilder(tagsInfo);
            expectedTree = new RootTreeNode();
        }

        [Test]
        public void TestBuildTree_OnTextToken()
        {
            var tokens = new[] { "a" };
            AddChild(expectedTree, new TextTreeNode("a"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSpaceToken()
        {
            var tokens = new[] { " " };
            AddChild(expectedTree, new SpaceTreeNode());

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnShortTag()
        {
            var tokens = new[] { "_" };
            AddChild(expectedTree, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnLongTag()
        {
            var tokens = new[] { "__" };
            AddChild(expectedTree, new TextTreeNode("__"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnDoubleEscapeSymbol()
        {
            var tokens = new[] { "\\", "\\" };
            AddChild(expectedTree, new TextTreeNode("\\"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnShortTagWithText()
        {
            var tokens = new[] { "_", "a", "_" };
            var tag = AddChild(expectedTree, new TagTreeNode(new ItalicTagInfo()));
            AddChild(tag, new TextTreeNode("a"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnNotClosedShortTag()
        {
            var tokens = new[] { "_", "a" };
            AddChild(expectedTree, new TextTreeNode("_"));
            AddChild(expectedTree, new TextTreeNode("a"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnNotOpenedShortTag()
        {
            var tokens = new[] { "a", "_" };
            AddChild(expectedTree, new TextTreeNode("a"));
            AddChild(expectedTree, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSpaceAfterOpeningShortTag()
        {
            var tokens = new[] { "_", " ", "a", "_" };
            AddChild(expectedTree, new TextTreeNode("_"));
            AddChild(expectedTree, new SpaceTreeNode());
            AddChild(expectedTree, new TextTreeNode("a"));
            AddChild(expectedTree, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSpaceBeforeClosingShortTag()
        {
            var tokens = new[] { "_", "a", " ", "_" };
            AddChild(expectedTree, new TextTreeNode("_"));
            AddChild(expectedTree, new TextTreeNode("a"));
            AddChild(expectedTree, new SpaceTreeNode());
            AddChild(expectedTree, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnOpeningShortTagInText()
        {
            var tokens = new[] { "a", "_", "b", "_" };
            AddChild(expectedTree, new TextTreeNode("a"));
            AddChild(expectedTree, new TextTreeNode("_"));
            AddChild(expectedTree, new TextTreeNode("b"));
            AddChild(expectedTree, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnClosingShortTagInText()
        {
            var tokens = new[] { "_", "a", "_", "b" };
            AddChild(expectedTree, new TextTreeNode("_"));
            AddChild(expectedTree, new TextTreeNode("a"));
            AddChild(expectedTree, new TextTreeNode("_"));
            AddChild(expectedTree, new TextTreeNode("b"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapeBeforeOpeningShortTag()
        {
            var tokens = new[] { "\\", "_", "a", "_" };
            AddChild(expectedTree, new TextTreeNode("_"));
            AddChild(expectedTree, new TextTreeNode("a"));
            AddChild(expectedTree, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapeBeforeClosingShortTag()
        {
            var tokens = new[] { "_", "a", "\\", "_" };
            AddChild(expectedTree, new TextTreeNode("_"));
            AddChild(expectedTree, new TextTreeNode("a"));
            AddChild(expectedTree, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapedOpeningSortTagInsideShortTag()
        {
            var tokens = new[] { "_", "a", " ", "\\", "_", "b", "_" };
            var tag = AddChild(expectedTree, new TagTreeNode(new ItalicTagInfo()));
            AddChild(tag, new TextTreeNode("a"));
            AddChild(tag, new SpaceTreeNode());
            AddChild(tag, new TextTreeNode("_"));
            AddChild(tag, new TextTreeNode("b"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapedClosingSortTagInsideShortTag()
        {
            var tokens = new[] { "_", "a", " ", "b", "\\", "_", "_" };
            var tag = AddChild(expectedTree, new TagTreeNode(new ItalicTagInfo()));
            AddChild(tag, new TextTreeNode("a"));
            AddChild(tag, new SpaceTreeNode());
            AddChild(tag, new TextTreeNode("b"));
            AddChild(tag, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSameOpeningBeforeShortTag()
        {
            var tokens = new[] { "_", "a", " ", "_", "b", "_" };
            var tag = AddChild(expectedTree, new TagTreeNode(new ItalicTagInfo()));
            AddChild(tag, new TextTreeNode("a"));
            AddChild(tag, new SpaceTreeNode());
            AddChild(tag, new TextTreeNode("_"));
            AddChild(tag, new TextTreeNode("b"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSameClosingShortTag()
        {
            var tokens = new[] { "_", "a", "_", " ", "b", "_" };
            var tag = AddChild(expectedTree, new TagTreeNode(new ItalicTagInfo()));
            AddChild(tag, new TextTreeNode("a"));
            AddChild(expectedTree, new SpaceTreeNode());
            AddChild(expectedTree, new TextTreeNode("b"));
            AddChild(expectedTree, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnShortTagInShortTag()
        {
            var tokens = new[] { "_", "a", " ", "_", "b", "_", " ", "c", "_" };
            var tag = AddChild(expectedTree, new TagTreeNode(new ItalicTagInfo()));
            AddChild(tag, new TextTreeNode("a"));
            AddChild(tag, new SpaceTreeNode());
            AddChild(tag, new TextTreeNode("_"));
            AddChild(tag, new TextTreeNode("b"));
            AddChild(expectedTree, new SpaceTreeNode());
            AddChild(expectedTree, new TextTreeNode("c"));
            AddChild(expectedTree, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnLongTagInLongTag()
        {
            var tokens = new[] { "__", "a", " ", "__", "b", "__", " ", "c", "__" };
            var tag = AddChild(expectedTree, new TagTreeNode(new BoldTagInfo()));
            AddChild(tag, new TextTreeNode("a"));
            AddChild(tag, new SpaceTreeNode());
            AddChild(tag, new TextTreeNode("__"));
            AddChild(tag, new TextTreeNode("b"));
            AddChild(expectedTree, new SpaceTreeNode());
            AddChild(expectedTree, new TextTreeNode("c"));
            AddChild(expectedTree, new TextTreeNode("__"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnLongTagInsideShortTag()
        {
            var tokens = new[] { "_", "a", " ", "__", "b", "__", " ", "c", "_" };
            var tag = AddChild(expectedTree, new TagTreeNode(new ItalicTagInfo()));
            AddChild(tag, new TextTreeNode("a"));
            AddChild(tag, new SpaceTreeNode());
            var innerText = AddChild(tag, new TagTreeNode(new BoldTagInfo()) {IsRaw = true});
            AddChild(innerText, new TextTreeNode("b"));
            AddChild(tag, new SpaceTreeNode());
            AddChild(tag, new TextTreeNode("c"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnShortTagInsideLongTag()
        {
            var tokens = new[] { "__", "a", " ", "_", "b", "_", " ", "c", "__" };
            var tag = AddChild(expectedTree, new TagTreeNode(new BoldTagInfo()));
            AddChild(tag, new TextTreeNode("a"));
            AddChild(tag, new SpaceTreeNode());
            var innerTag = AddChild(tag, new TagTreeNode(new ItalicTagInfo()));
            AddChild(innerTag, new TextTreeNode("b"));
            AddChild(tag, new SpaceTreeNode());
            AddChild(tag, new TextTreeNode("c"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnOuterTagClosedBeforeInner()
        {
            var tokens = new[] { "__", "a", " ", "_", "b", "__", " ", "c", "_" };
            var tag = AddChild(expectedTree, new TagTreeNode(new BoldTagInfo()));
            AddChild(tag, new TextTreeNode("a"));
            AddChild(tag, new SpaceTreeNode());
            AddChild(tag, new TextTreeNode("_"));
            AddChild(tag, new TextTreeNode("b"));
            AddChild(expectedTree, new SpaceTreeNode());
            AddChild(expectedTree, new TextTreeNode("c"));
            AddChild(expectedTree, new TextTreeNode("_"));

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        private static TokenTreeNode AddChild(TokenTreeNode parent, TokenTreeNode child)
        {
            parent.Children.Add(child);
            return child;
        }
    }
}