using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MarkdownTokenTreeBuilderTests
    {
        // Ну здесь скорее всего нужен какой-то DataBuilder для деревьев
        // Он еще потом пригодится в тестах на Translator, чтобы генерировать входные данные

        private Dictionary<string, TagInfo> tagsInfo;
        private MarkdownTokenTreeBuilder builder;

        [SetUp]
        public void SetUp()
        {
            tagsInfo = new Dictionary<string, TagInfo>
            {
                ["_"] = new TagInfo("_", true, "_"),
                ["__"] = new TagInfo("__", false, "__")
            };
            builder = new MarkdownTokenTreeBuilder(tagsInfo);
        }

        [Test]
        public void TestBuildTree_OnTextToken()
        {
            var tokens = new[] { "a" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "a");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSpaceToken()
        {
            var tokens = new[] { " " };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Space, " ");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnShortTag()
        {
            var tokens = new[] { "_" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnLongTag()
        {
            var tokens = new[] { "__" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "__");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnDoubleEscapeSymbol()
        {
            var tokens = new[] { "\\", "\\" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.EscapeSymbol, "\\");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnShortTagWithText()
        {
            var tokens = new[] { "_", "a", "_" };
            var expectedTree = new TokenTreeNode();
            var tag = AddChild(expectedTree, TokenType.Tag, "_");
            AddChild(tag, TokenType.Text, "a");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnNotClosedShortTag()
        {
            var tokens = new[] { "_", "a" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "_");
            AddChild(expectedTree, TokenType.Text, "a");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnNotOpenedShortTag()
        {
            var tokens = new[] { "a", "_" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "a");
            AddChild(expectedTree, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSpaceAfterOpeningShortTag()
        {
            var tokens = new[] { "_", " ", "a", "_" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "_");
            AddChild(expectedTree, TokenType.Space, " ");
            AddChild(expectedTree, TokenType.Text, "a");
            AddChild(expectedTree, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSpaceBeforeClosingShortTag()
        {
            var tokens = new[] { "_", "a", " ", "_" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "_");
            AddChild(expectedTree, TokenType.Text, "a");
            AddChild(expectedTree, TokenType.Space, " ");
            AddChild(expectedTree, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnOpeningShortTagInText()
        {
            var tokens = new[] { "a", "_", "b", "_" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "a");
            AddChild(expectedTree, TokenType.Text, "_");
            AddChild(expectedTree, TokenType.Text, "b");
            AddChild(expectedTree, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnClosingShortTagInText()
        {
            var tokens = new[] { "_", "a", "_", "b" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "_");
            AddChild(expectedTree, TokenType.Text, "a");
            AddChild(expectedTree, TokenType.Text, "_");
            AddChild(expectedTree, TokenType.Text, "b");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapeBeforeOpeningShortTag()
        {
            var tokens = new[] { "\\", "_", "a", "_" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "_");
            AddChild(expectedTree, TokenType.Text, "a");
            AddChild(expectedTree, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapeBeforeClosingShortTag()
        {
            var tokens = new[] { "_", "a", "\\", "_" };
            var expectedTree = new TokenTreeNode();
            AddChild(expectedTree, TokenType.Text, "_");
            AddChild(expectedTree, TokenType.Text, "a");
            AddChild(expectedTree, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapedOpeningSortTagInsideShortTag()
        {
            var tokens = new[] { "_", "a", " ", "\\", "_", "b", "_" };
            var expectedTree = new TokenTreeNode();
            var tag = AddChild(expectedTree, TokenType.Tag, "_");
            AddChild(tag, TokenType.Text, "a");
            AddChild(tag, TokenType.Space, " ");
            AddChild(tag, TokenType.Text, "_");
            AddChild(tag, TokenType.Text, "b");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnEscapedClosingSortTagInsideShortTag()
        {
            var tokens = new[] { "_", "a", " ", "b", "\\", "_", "_" };
            var expectedTree = new TokenTreeNode();
            var tag = AddChild(expectedTree, TokenType.Tag, "_");
            AddChild(tag, TokenType.Text, "a");
            AddChild(tag, TokenType.Space, " ");
            AddChild(tag, TokenType.Text, "b");
            AddChild(tag, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSameOpeningBeforeShortTag()
        {
            var tokens = new[] { "_", "a", " ", "_", "b", "_" };
            var expectedTree = new TokenTreeNode();
            var tag = AddChild(expectedTree, TokenType.Tag, "_");
            AddChild(tag, TokenType.Text, "a");
            AddChild(tag, TokenType.Space, " ");
            AddChild(tag, TokenType.Text, "_");
            AddChild(tag, TokenType.Text, "b");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnSameClosingShortTag()
        {
            var tokens = new[] { "_", "a", "_", " ", "b", "_" };
            var expectedTree = new TokenTreeNode();
            var tag = AddChild(expectedTree, TokenType.Tag, "_");
            AddChild(tag, TokenType.Text, "a");
            AddChild(expectedTree, TokenType.Space, " ");
            AddChild(expectedTree, TokenType.Text, "b");
            AddChild(expectedTree, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnShortTagInShortTag()
        {
            var tokens = new[] { "_", "a", " ", "_", "b", "_", " ", "c", "_" };
            var expectedTree = new TokenTreeNode();
            var tag = AddChild(expectedTree, TokenType.Tag, "_");
            AddChild(tag, TokenType.Text, "a");
            AddChild(tag, TokenType.Space, " ");
            AddChild(tag, TokenType.Text, "_");
            AddChild(tag, TokenType.Text, "b");
            AddChild(expectedTree, TokenType.Space, " ");
            AddChild(expectedTree, TokenType.Text, "c");
            AddChild(expectedTree, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnLongTagInLongTag()
        {
            var tokens = new[] { "__", "a", " ", "__", "b", "__", " ", "c", "__" };
            var expectedTree = new TokenTreeNode();
            var tag = AddChild(expectedTree, TokenType.Tag, "__");
            AddChild(tag, TokenType.Text, "a");
            AddChild(tag, TokenType.Space, " ");
            AddChild(tag, TokenType.Text, "__");
            AddChild(tag, TokenType.Text, "b");
            AddChild(expectedTree, TokenType.Space, " ");
            AddChild(expectedTree, TokenType.Text, "c");
            AddChild(expectedTree, TokenType.Text, "__");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnLongTagInsideShortTag()
        {
            var tokens = new[] { "_", "a", " ", "__", "b", "__", " ", "c", "_" };
            var expectedTree = new TokenTreeNode();
            var tag = AddChild(expectedTree, TokenType.Tag, "_");
            AddChild(tag, TokenType.Text, "a");
            AddChild(tag, TokenType.Space, " ");
            var innerText = AddChild(tag, TokenType.Tag, "__", true);
            AddChild(innerText, TokenType.Text, "b");
            AddChild(tag, TokenType.Space, " ");
            AddChild(tag, TokenType.Text, "c");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnShortTagInsideLongTag()
        {
            var tokens = new[] { "__", "a", " ", "_", "b", "_", " ", "c", "__" };
            var expectedTree = new TokenTreeNode();
            var tag = AddChild(expectedTree, TokenType.Tag, "__");
            AddChild(tag, TokenType.Text, "a");
            AddChild(tag, TokenType.Space, " ");
            var innerTag = AddChild(tag, TokenType.Tag, "_");
            AddChild(innerTag, TokenType.Text, "b");
            AddChild(tag, TokenType.Space, " ");
            AddChild(tag, TokenType.Text, "c");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public void TestBuildTree_OnOuterTagClosedBeforeInner()
        {
            var tokens = new[] { "__", "a", " ", "_", "b", "__", " ", "c", "_" };
            var expectedTree = new TokenTreeNode();
            var tag = AddChild(expectedTree, TokenType.Tag, "__");
            AddChild(tag, TokenType.Text, "a");
            AddChild(tag, TokenType.Space, " ");
            AddChild(tag, TokenType.Text, "_");
            AddChild(tag, TokenType.Text, "b");
            AddChild(expectedTree, TokenType.Space, " ");
            AddChild(expectedTree, TokenType.Text, "c");
            AddChild(expectedTree, TokenType.Text, "_");

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }

        private static TokenTreeNode AddChild(TokenTreeNode parent, TokenType type, string text, bool isRaw = false)
        {
            var child = new TokenTreeNode(type, text) { IsRaw = isRaw };
            parent.Children.Add(child);
            return child;
        }
    }
}