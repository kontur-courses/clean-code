using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Rules;
using Markdown.SyntaxAnalysis.SyntaxTreeRealization;
using Markdown.Tokenization;

namespace Markdown.SyntaxAnalysis.SyntaxTreeBuilders
{
    public class MarkdownSyntaxTreeBuilder : ISyntaxTreeBuilder
    {
        private readonly IRules rules;
        private readonly List<SyntaxTreeNode> separatorStack;

        public MarkdownSyntaxTreeBuilder(IRules rules)
        {
            this.rules = rules;
            separatorStack = new List<SyntaxTreeNode>();
        }

        public SyntaxTree BuildSyntaxTree(IEnumerable<Token> tokens, string text)
        {
            var syntaxTree = new SyntaxTree {Root = new SyntaxTreeNode(new Token(0, string.Empty, false))};
            tokens.Aggregate(syntaxTree.Root,
                (current, token) => ProcessToken(token, syntaxTree, current, text));

            separatorStack.Clear();

            return syntaxTree;
        }

        private SyntaxTreeNode ProcessToken(Token token, SyntaxTree syntaxTree, SyntaxTreeNode currentNode,
            string text)
        {
            if (token.IsSeparator && IsValidSeparator(token, text))
            {
                return ProcessSeparatorToken(token, syntaxTree, currentNode);
            }

            if (token.IsSeparator)
            {
                token = new Token(token.Position, token.Value, false);
            }

            currentNode.AddChild(new SyntaxTreeNode(token));
            return currentNode;
        }

        private SyntaxTreeNode ProcessSeparatorToken(Token token, SyntaxTree syntaxTree,
            SyntaxTreeNode currentNode)
        {
            switch (token.Value)
            {
                case "_":
                    return ProcessPairedSeparatorToken(token, syntaxTree, currentNode);
                case "\\_":
                    return ProcessCommentedSeparator(token, currentNode);
                case "__":
                    return ProcessPairedSeparatorToken(token, syntaxTree, currentNode);
                case "\\__":
                    return ProcessCommentedSeparator(token, currentNode);
                default:
                    throw new NotImplementedException();
            }
        }

        private SyntaxTreeNode ProcessCommentedSeparator(Token token, SyntaxTreeNode currentNode)
        {
            var newToken = new Token(token.Position + 1, token.Value.Substring(1), false);
            var tokenNode = new SyntaxTreeNode(newToken);
            currentNode.AddChild(tokenNode);
            return currentNode;
        }

        private SyntaxTreeNode ProcessPairedSeparatorToken(Token token, SyntaxTree syntaxTree,
            SyntaxTreeNode currentNode)
        {
            var separatorNode = new SyntaxTreeNode(token);
            currentNode.AddChild(separatorNode);

            if (IsEndSeparator(token))
            {
                var openSeparatorNode = separatorStack.Last(s => s.Token.Value == token.Value);
                separatorStack.Remove(openSeparatorNode);
                return separatorStack.Count == 0 ? syntaxTree.Root : separatorStack.Last();
            }

            separatorStack.Add(separatorNode);
            return separatorNode;
        }

        private bool IsEndSeparator(Token token)
        {
            return separatorStack.Count > 0 && separatorStack.Any(s => s.Token.Value == token.Value);
        }

        private bool IsValidSeparator(Token token, string text)
        {
            var isFirst = !IsEndSeparator(token);
            var hasParentSeparator = separatorStack.Count > 0 && separatorStack.First().Token.Value != token.Value;
            return token.IsSeparator && hasParentSeparator
                ? rules.IsSeparatorValid(text, token.Position, isFirst, token.Value.Length,
                    separatorStack.Last().Token.Value)
                : rules.IsSeparatorValid(text, token.Position, isFirst, token.Value.Length);
        }
    }
}