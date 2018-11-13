using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MarkdownTokenTreeBuilderTests
    {
        private static IEnumerable TreeTestCases
        {
            get
            {
                // Ну здесь скорее всего нужен какой-то DataBuilder для деревьев
                // Он еще потом пригодится в тестах на Translator, чтобы генерировать входные данные

                var tokens = new[] { "a" };
                var root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "a");
                yield return new TestCaseData(tokens, root).SetName("Text");

                tokens = new[] { " " };
                root = new TokenTreeNode();
                AddChild(root, TokenType.Space, " ");
                yield return new TestCaseData(tokens, root).SetName("TextAndSpace");

                tokens = new[] { "_" };
                root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("ShortTagAsText");

                tokens = new[] { "__" };
                root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "__");
                yield return new TestCaseData(tokens, root).SetName("LongTagAsText");

                tokens = new[] { "\\", "\\" };
                root = new TokenTreeNode();
                AddChild(root, TokenType.EscapeSymbol, "\\");
                yield return new TestCaseData(tokens, root).SetName("DoubleEscapeSymbol");

                tokens = new[] { "_", "a", "_" };
                root = new TokenTreeNode();
                var tag = AddChild(root, TokenType.Tag, "_");
                AddChild(tag, TokenType.Text, "a");
                yield return new TestCaseData(tokens, root).SetName("ShortTagWithText");

                tokens = new[] { "_", "a" };
                root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "_");
                AddChild(root, TokenType.Text, "a");
                yield return new TestCaseData(tokens, root).SetName("NotClosedShortTag");

                tokens = new[] { "a", "_" };
                root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "a");
                AddChild(root, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("NotOpenedShortTag");

                tokens = new[] { "_", " ", "a", "_" };
                root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "_");
                AddChild(root, TokenType.Space, " ");
                AddChild(root, TokenType.Text, "a");
                AddChild(root, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("SpaceAfterOpeningShortTag");

                tokens = new[] { "_", "a", " ", "_" };
                root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "_");
                AddChild(root, TokenType.Text, "a");
                AddChild(root, TokenType.Space, " ");
                AddChild(root, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("SpaceBeforeClosingShortTag");

                tokens = new[] { "a", "_", "b", "_"};
                root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "a");
                AddChild(root, TokenType.Text, "_");
                AddChild(root, TokenType.Text, "b");
                AddChild(root, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("OpeningShortTagInText");

                tokens = new[] { "_", "a", "_", "b" };
                root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "_");
                AddChild(root, TokenType.Text, "a");
                AddChild(root, TokenType.Text, "_");
                AddChild(root, TokenType.Text, "b");
                yield return new TestCaseData(tokens, root).SetName("ClosingShortTagInText");

                tokens = new[] { "\\", "_", "a", "_" };
                root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "_");
                AddChild(root, TokenType.Text, "a");
                AddChild(root, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("EscapeBeforeOpeningShortTag");

                tokens = new[] { "_", "a", "\\", "_" };
                root = new TokenTreeNode();
                AddChild(root, TokenType.Text, "_");
                AddChild(root, TokenType.Text, "a");
                AddChild(root, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("EscapeBeforeClosingShortTag");

                tokens = new[] { "_", "a", " ", "\\", "_", "b", "_" };
                root = new TokenTreeNode();
                tag = AddChild(root, TokenType.Tag, "_");
                AddChild(tag, TokenType.Text, "a");
                AddChild(tag, TokenType.Space, " ");
                AddChild(tag, TokenType.Text, "_");
                AddChild(tag, TokenType.Text, "b");
                yield return new TestCaseData(tokens, root).SetName("EscapedOpeningSortTagInsideShortTag");

                tokens = new[] { "_", "a", " ", "b", "\\", "_", "_" };
                root = new TokenTreeNode();
                tag = AddChild(root, TokenType.Tag, "_");
                AddChild(tag, TokenType.Text, "a");
                AddChild(tag, TokenType.Space, " ");
                AddChild(tag, TokenType.Text, "b");
                AddChild(tag, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("EscapedClosingSortTagInsideShortTag");

                tokens = new[] { "_", "a", " ", "_" , "b", "_" };
                root = new TokenTreeNode();
                tag = AddChild(root, TokenType.Tag, "_");
                AddChild(tag, TokenType.Text, "a");
                AddChild(tag, TokenType.Space, " ");
                AddChild(tag, TokenType.Text, "_");
                AddChild(tag, TokenType.Text, "b");
                yield return new TestCaseData(tokens, root).SetName("SameOpeningBeforeShortTag");

                tokens = new[] { "_", "a", "_", " ", "b", "_" };
                root = new TokenTreeNode();
                tag = AddChild(root, TokenType.Tag, "_");
                AddChild(tag, TokenType.Text, "a");
                AddChild(root, TokenType.Space, " ");
                AddChild(root, TokenType.Text, "b");
                AddChild(root, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("SameClosingShortTag");

                tokens = new[] { "_", "a", " ", "_", "b", "_", " ", "c", "_" };
                root = new TokenTreeNode();
                tag = AddChild(root, TokenType.Tag, "_");
                AddChild(tag, TokenType.Text, "a");
                AddChild(tag, TokenType.Space, " ");
                AddChild(tag, TokenType.Text, "_");
                AddChild(tag, TokenType.Text, "b");
                AddChild(root, TokenType.Space, " ");
                AddChild(root, TokenType.Text, "c");
                AddChild(root, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("ShortTagInShortTag");

                tokens = new[] { "__", "a", " ", "__", "b", "__", " ", "c", "__" };
                root = new TokenTreeNode();
                tag = AddChild(root, TokenType.Tag, "__");
                AddChild(tag, TokenType.Text, "a");
                AddChild(tag, TokenType.Space, " ");
                AddChild(tag, TokenType.Text, "__");
                AddChild(tag, TokenType.Text, "b");
                AddChild(root, TokenType.Space, " ");
                AddChild(root, TokenType.Text, "c");
                AddChild(root, TokenType.Text, "__");
                yield return new TestCaseData(tokens, root).SetName("LongTagInLongTag");

                tokens = new[] { "_", "a", " ", "__", "b", "__", " ", "c", "_" };
                root = new TokenTreeNode();
                tag = AddChild(root, TokenType.Tag, "_");
                AddChild(tag, TokenType.Text, "a");
                AddChild(tag, TokenType.Space, " ");
                var innerText = AddChild(tag, TokenType.Tag, "__", true);
                AddChild(innerText, TokenType.Text, "b");
                AddChild(tag, TokenType.Space, " ");
                AddChild(tag, TokenType.Text, "c");
                yield return new TestCaseData(tokens, root).SetName("LongTagInsideShortTag");

                tokens = new[] { "__", "a", " ", "_", "b", "_", " ", "c", "__" };
                root = new TokenTreeNode();
                tag = AddChild(root, TokenType.Tag, "__");
                AddChild(tag, TokenType.Text, "a");
                AddChild(tag, TokenType.Space, " ");
                var innerTag = AddChild(tag, TokenType.Tag, "_");
                AddChild(innerTag, TokenType.Text, "b");
                AddChild(tag, TokenType.Space, " ");
                AddChild(tag, TokenType.Text, "c");
                yield return new TestCaseData(tokens, root).SetName("ShortTagInsideLongTag");

                tokens = new[] { "__", "a", " ", "_", "b", "__", " ", "c", "_" };
                root = new TokenTreeNode();
                tag = AddChild(root, TokenType.Tag, "__");
                AddChild(tag, TokenType.Text, "a");
                AddChild(tag, TokenType.Space, " ");
                AddChild(tag, TokenType.Text, "_");
                AddChild(tag, TokenType.Text, "b");
                AddChild(root, TokenType.Space, " ");
                AddChild(root, TokenType.Text, "c");
                AddChild(root, TokenType.Text, "_");
                yield return new TestCaseData(tokens, root).SetName("OuterTagClosesBeforeInner");
            }
        }

        private static TokenTreeNode AddChild(TokenTreeNode parent, TokenType type, string text, bool isRaw = false)
        {
            var child = new TokenTreeNode(type, text) {IsRaw = isRaw};
            parent.Children.Add(child);
            return child;
        }

        [TestCaseSource(nameof(TreeTestCases))]
        public void TestBuildTree(IEnumerable<string> tokens, TokenTreeNode expectedTree)
        {
            var tagsInfo = new Dictionary<string, TagInfo>
            {
                ["_"] = new TagInfo("_", true, "_"),
                ["__"] = new TagInfo("__", false, "__")
            };
            var builder = new MarkdownTokenTreeBuilder(tagsInfo);

            var tree = builder.BuildTree(tokens);

            tree.Should().BeEquivalentTo(expectedTree);
        }
    }
}