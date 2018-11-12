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
            var nodeStack = new Stack<TokenTreeNode>();
            nodeStack.Push(new TokenTreeNode());
            var tokensArray = tokens as string[] ?? tokens.ToArray();
            for (var i = 0; i < tokensArray.Length; i++)
            {
                var token = tokensArray[i];
                var lastNode = nodeStack.Peek();
                var tokenType = GetTokenType(token);
                var previousTokenType = i > 0 ? GetTokenType(tokensArray[i - 1]) : TokenType.Space;
                if (tokenType == TokenType.Space || tokenType == TokenType.Text || previousTokenType == TokenType.EscapeSymbol)
                {
                    lastNode.Children.Add(new TokenTreeNode(tokenType == TokenType.Tag ? TokenType.Text : tokenType, token, lastNode));
                    continue;
                }
                if (tokenType == TokenType.Tag)
                    ProcessTagToken(nodeStack, tokensArray, i, lastNode, previousTokenType);
            }
            var root = nodeStack.Count > 1 ? GetTreeRoot(nodeStack) : nodeStack.Peek();
            foreach (var child in root.Children)
                FixInnerTags(child);
            return root;
        }

        private void ProcessTagToken(Stack<TokenTreeNode> nodeStack, string[] tokensArray, int i, TokenTreeNode lastNode, TokenType previousTokenType)
        {
            var token = tokensArray[i];
            var nextTokenIsSpace = i == tokensArray.Length - 1 || GetTokenType(tokensArray[i + 1]) == TokenType.Space;
            var tagIsOpened = nodeStack.Any(node => node.Type == TokenType.Tag && tagsInfo[node.Text].ClosingTag == token);
            if (previousTokenType == TokenType.Space && !nextTokenIsSpace)
            {
                OpenTag(nodeStack, lastNode, token);
            }
            else if (tagIsOpened && previousTokenType != TokenType.EscapeSymbol && 
                     previousTokenType != TokenType.Space && nextTokenIsSpace)
            {
                CloseTag(nodeStack, token);
            }
            else
            {
                lastNode.Children.Add(new TokenTreeNode(TokenType.Text, token, lastNode));
            }
        }

        private static void OpenTag(Stack<TokenTreeNode> nodeStack, TokenTreeNode lastNode, string token)
        {
            if (nodeStack.Any(node => node.Type == TokenType.Tag && node.Text == token))
                RemoveOpenedTag(nodeStack, token);
            nodeStack.Push(new TokenTreeNode(TokenType.Tag, token, lastNode));
        }

        private void CloseTag(Stack<TokenTreeNode> nodeStack, string token)
        {
            while (tagsInfo[nodeStack.Peek().Text].ClosingTag != token)
                ShiftStack(nodeStack);
            var tag = nodeStack.Pop();
            nodeStack.Peek().Children.Add(tag);
        }

        private static void RemoveOpenedTag(Stack<TokenTreeNode> nodeStack, string tagToClose)
        {
            var tempStack = new Stack<TokenTreeNode>();
            while (nodeStack.Peek().Type != TokenType.Tag && nodeStack.Peek().Text != tagToClose)
            {
                tempStack.Push(nodeStack.Pop());
            }
            var tokenToRemove = nodeStack.Pop();
            var parent = tokenToRemove.Parent;
            parent.Children.Add(new TokenTreeNode(TokenType.Text, tokenToRemove.Text, parent));
            parent.AddChildren(tokenToRemove.Children);
            while (tempStack.Count > 0)
            {
                nodeStack.Push(tempStack.Pop());
            }
        }

        private void FixInnerTags(TokenTreeNode node, bool isInTag = false)
        {
            if (isInTag && node.Type == TokenType.Tag && !tagsInfo[node.Text].CanBeInsideOtherTag)
                node.Type = TokenType.Text;
            foreach (var child in node.Children.Where(token => token.Type == TokenType.Tag))
                FixInnerTags(child, true);
        }

        private static TokenTreeNode GetTreeRoot(Stack<TokenTreeNode> nodeStack)
        {
            TokenTreeNode root;
            do
            {
                root = ShiftStack(nodeStack);
            } while (nodeStack.Count > 1);
            return root;
        }

        private static TokenTreeNode ShiftStack(Stack<TokenTreeNode> nodeStack)
        {
            var previousNode = nodeStack.Pop();
            var parent = nodeStack.Peek();
            parent.Children.Add(new TokenTreeNode(TokenType.Text, previousNode.Text, parent));
            parent.AddChildren(previousNode.Children);
            return parent;
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