using System.Collections.Generic;
using FluentAssertions;
using Markdown.Rules;
using Markdown.SyntaxAnalysis.SyntaxTreeRealization;
using Markdown.Tokenization;
using NUnit.Framework;

namespace Markdown.SyntaxAnalysis.SyntaxTreeBuilders.Tests
{
    [TestFixture]
    public class MarkdownSyntaxTreeBuilderTests
    {
        private MarkdownSyntaxTreeBuilder builder;

        [SetUp]
        public void SetUp()
        {
            builder = new MarkdownSyntaxTreeBuilder(new TestRules());
        }


        [Test]
        public void BuildSyntaxTree_ShouldReturnOneDepthTree_WhenNoSeparatorsInTokens()
        {
            var tokens = new List<Token>
            {
                new Token(0, "sample text", false)
            };

            var tree = builder.BuildSyntaxTree(tokens, "sample text");

            var expectedEmptyTree = GetExpectedEmptySyntaxTree();
            expectedEmptyTree.Root.AddChild(new SyntaxTreeNode(new Token(0, "sample text", false)));
            tree.ShouldBeEquivalentTo(expectedEmptyTree);
        }

        [Test]
        public void BuildSyntaxTree_ShouldReturnTwoDepth_WhenOneSeparatorInTokens()
        {
            var tokens = new List<Token>
            {
                new Token(0, "samplez", false),
                new Token(7, "_", true),
                new Token(8, "textz", false),
                new Token(13, "_", true)
            };

            var tree = builder.BuildSyntaxTree(tokens, "samplez_textz_");

            var expectedEmptyTree = GetExpectedEmptySyntaxTree();
            var separatorNode = new SyntaxTreeNode(new Token(7, "_", true));
            AddChildrenToNode(expectedEmptyTree.Root, new Token(0, "samplez", false));
            expectedEmptyTree.Root.AddChild(separatorNode);
            AddChildrenToNode(separatorNode, new Token(8, "textz", false), new Token(13, "_", true));

            tree.ShouldBeEquivalentTo(expectedEmptyTree);
        }

        [Test]
        public void BuildSyntaxTree_ShouldChangeTokenToNoSeparator_WhenInvalidSeparatorInTokens()
        {
            var tokens = new List<Token>
            {
                new Token(0, "_", true),
                new Token(1, "sample", false),
                new Token(7, "_", true),
                new Token(8, "text", false)
            };

            var tree = builder.BuildSyntaxTree(tokens, "_sample_text");

            var expectedEmptyTree = GetExpectedEmptySyntaxTree();
            AddChildrenToNode(expectedEmptyTree.Root, new Token(0, "_", false),
                new Token(1, "sample", false));
            var separatorNode = new SyntaxTreeNode(new Token(7, "_", true));
            AddChildrenToNode(separatorNode, new Token(8, "text", false));
            expectedEmptyTree.Root.AddChild(separatorNode);

            tree.ShouldBeEquivalentTo(expectedEmptyTree);
        }

        private void AddChildrenToNode(SyntaxTreeNode node, params Token[] tokens)
        {
            foreach (var token in tokens)
            {
                node.AddChild(new SyntaxTreeNode(token));
            }
        }

        private SyntaxTree GetExpectedEmptySyntaxTree()
        {
            return new SyntaxTree {Root = new SyntaxTreeNode(new Token(0, "", false))};
        }
    }

    public class TestRules : IRules
    {
        public bool IsSeparatorValid(string text, int position, bool isFirst)
        {
            return position % 2 == 1;
        }
    }
}