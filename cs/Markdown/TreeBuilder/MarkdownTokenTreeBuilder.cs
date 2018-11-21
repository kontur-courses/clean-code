using System.Collections.Generic;
using System.Linq;
using Markdown.Data;
using Markdown.Data.Nodes;
using Markdown.Data.TagsInfo;

namespace Markdown.TreeBuilder
{
    public class MarkdownTokenTreeBuilder : ITokenTreeBuilder
    {
        private readonly Dictionary<string, ITagInfo> tagsInfo = new Dictionary<string, ITagInfo>();

        public MarkdownTokenTreeBuilder(IEnumerable<ITagInfo> tagsInfo)
        {
            foreach (var tagInfo in tagsInfo)
            {
                this.tagsInfo[tagInfo.OpeningTag] = tagInfo;
                this.tagsInfo[tagInfo.ClosingTag] = tagInfo;
            }
        }

        public TokenTreeNode BuildTree(IEnumerable<Token> tokens)
        {
            var openedTags = new Stack<TagTreeNode>();
            openedTags.Push(new RootTreeNode());
            var tokensArray = tokens as Token[] ?? tokens.ToArray();
            for (var i = 0; i < tokensArray.Length; i++)
            {
                var token = tokensArray[i];
                var previousTokenType = i > 0 ? tokensArray[i - 1].Type : TokenType.ParagraphStart;
                var nextTokenType = i < tokensArray.Length - 1 ? tokensArray[i + 1].Type : TokenType.ParagraphEnd;

                AddTokenToTree(openedTags, token, previousTokenType, nextTokenType);
            }
            var root = GetTreeRoot(openedTags);
            FixTagsNesting(root);
            return root;
        }

        private void AddTokenToTree(Stack<TagTreeNode> openedTags, Token token, TokenType previousTokenType, TokenType nextTokenType)
        {
            var currentTag = openedTags.Peek();

            switch (token.Type)
            {
                case TokenType.Space:
                    currentTag.Children.Add(new TextTreeNode(" "));
                    break;
                case TokenType.Text:
                    currentTag.Children.Add(new TextTreeNode(token.Text));
                    break;
                case TokenType.Tag:
                    AddTagToken(openedTags, token.Text, previousTokenType, nextTokenType);
                    break;
            }
        }

        private void AddTagToken(Stack<TagTreeNode> openedTags, string token, TokenType previousTokenType, TokenType nexTokenType)
        {
            var tagIsOpened = openedTags.Any(node => node.TagInfo?.ClosingTag == token);
            var tagInfo = tagsInfo[token];

            if (tagInfo.MustBeOpened(token, tagIsOpened, previousTokenType, nexTokenType))
                openedTags.Push(new TagTreeNode(tagInfo));
            else if (tagInfo.MustBeClosed(token, tagIsOpened, previousTokenType, nexTokenType))
                CloseTag(openedTags, token);
            else
                openedTags.Peek().Children.Add(new TextTreeNode(token));
        }

        private static void CloseTag(Stack<TagTreeNode> nodeStack, string token)
        {
            while (nodeStack.Peek().TagInfo.ClosingTag != token)
                CancelLastOpenedTag(nodeStack);
            var tag = nodeStack.Pop();
            nodeStack.Peek().Children.Add(tag);
        }

        private static void FixTagsNesting(TokenTreeNode node, bool isInTag = false)
        {
            if (!(node is TagTreeNode tagNode))
                return;
            if (isInTag && !tagNode.TagInfo.CanBeInsideOtherTag)
                tagNode.IsRaw = true;
            foreach (var child in node.Children)
                FixTagsNesting(child, !(tagNode is RootTreeNode));
        }

        private static TokenTreeNode GetTreeRoot(Stack<TagTreeNode> openedTags)
        {
            if (openedTags.Count == 1)
                return openedTags.Peek();
            while (openedTags.Count > 1)
                CancelLastOpenedTag(openedTags);
            return openedTags.Peek();
        }

        private static void CancelLastOpenedTag(Stack<TagTreeNode> nodeStack)
        {
            var previousNode = nodeStack.Pop();
            var parent = nodeStack.Peek();
            parent.Children.Add(new TextTreeNode(previousNode.TagInfo.OpeningTag));
            parent.Children.AddRange(previousNode.Children);
        }
    }
}