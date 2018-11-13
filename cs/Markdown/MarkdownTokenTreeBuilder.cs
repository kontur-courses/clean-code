using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkdownTokenTreeBuilder : ITokenTreeBuilder
    {
        private readonly Dictionary<string, TagInfo> tagsInfo;

        public MarkdownTokenTreeBuilder(Dictionary<string, TagInfo> tagsInfo)
        {
            this.tagsInfo = tagsInfo;
        }

        public TokenTreeNode BuildTree(IEnumerable<string> tokens)
        {
            var openedTags = new Stack<TokenTreeNode>();
            openedTags.Push(new TokenTreeNode());
            var tokensArray = tokens as string[] ?? tokens.ToArray();
            for (var i = 0; i < tokensArray.Length; i++)
            {
                var token = tokensArray[i];
                var tokenType = GetTokenType(token);
                var previousTokenType = i > 0 ? GetTokenType(tokensArray[i - 1]) : TokenType.Space;
                var nextTokenType = i < tokensArray.Length - 1 ? GetTokenType(tokensArray[i + 1]) : TokenType.Space;

                if (tokenType == TokenType.Space || tokenType == TokenType.Text || previousTokenType == TokenType.EscapeSymbol)
                    openedTags.Peek().Children.Add(new TokenTreeNode(tokenType == TokenType.Tag ? TokenType.Text : tokenType, token));
                else if (tokenType == TokenType.Tag)
                    ProcessTagToken(openedTags, token, previousTokenType, nextTokenType);
            }
            var root = GetTreeRoot(openedTags);
            foreach (var child in root.Children)
                FixInnerTags(child);
            return root;
        }

        private void ProcessTagToken(Stack<TokenTreeNode> openedTags, string token, TokenType previousTokenType, TokenType nexTokenType)
        {
            var tagIsOpened = openedTags.Any(node => node.Type == TokenType.Tag && tagsInfo[node.Text].ClosingTag == token);
            if (previousTokenType == TokenType.Space && nexTokenType != TokenType.Space && !tagIsOpened)
            {
                openedTags.Push(new TokenTreeNode(TokenType.Tag, token));
            }
            else if (tagIsOpened && nexTokenType == TokenType.Space &&
                     previousTokenType != TokenType.EscapeSymbol && previousTokenType != TokenType.Space)
            {
                CloseTag(openedTags, token);
            }
            else
            {
                openedTags.Peek().Children.Add(new TokenTreeNode(TokenType.Text, token));
            }
        }

        private void CloseTag(Stack<TokenTreeNode> nodeStack, string token)
        {
            while (tagsInfo[nodeStack.Peek().Text].ClosingTag != token)
                DeleteOpenedTag(nodeStack);
            var tag = nodeStack.Pop();
            nodeStack.Peek().Children.Add(tag);
        }

        private void FixInnerTags(TokenTreeNode node, bool isInTag = false)
        {
            if (isInTag && node.Type == TokenType.Tag && !tagsInfo[node.Text].CanBeInsideOtherTag)
                node.IsRaw = true;
            foreach (var child in node.Children.Where(token => token.Type == TokenType.Tag))
                FixInnerTags(child, true);
        }

        private static TokenTreeNode GetTreeRoot(Stack<TokenTreeNode> openedTags)
        {
            if (openedTags.Count == 1)
                return openedTags.Peek();
            while (openedTags.Count > 1)
                DeleteOpenedTag(openedTags);
            return openedTags.Peek();
        }

        private static void DeleteOpenedTag(Stack<TokenTreeNode> nodeStack)
        {
            var previousNode = nodeStack.Pop();
            var parent = nodeStack.Peek();
            parent.Children.Add(new TokenTreeNode(TokenType.Text, previousNode.Text));
            parent.Children.AddRange(previousNode.Children);
        }

        private TokenType GetTokenType(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("token should be not empty string");
            if (token == "\\")
                return TokenType.EscapeSymbol;
            if (string.IsNullOrWhiteSpace(token))
                return TokenType.Space;
            if (tagsInfo.ContainsKey(token) || tagsInfo.Any(pair => pair.Value.ClosingTag == token))
                return TokenType.Tag;
            return TokenType.Text;
        }
    }
}