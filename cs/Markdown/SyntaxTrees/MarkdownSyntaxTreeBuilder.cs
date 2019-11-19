using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Rules;
using Markdown.Tokens;

namespace Markdown.SyntaxTrees
{
    public class MarkdownSyntaxTreeBuilder : ISyntaxTreeBuilder
    {
        private readonly IRules rules;
        private readonly Stack<SyntaxTreeNode> separatorStack;

        public MarkdownSyntaxTreeBuilder(IRules rules)
        {
            this.rules = rules;
            separatorStack = new Stack<SyntaxTreeNode>();
        }

        public SyntaxTree BuildSyntaxTree(IEnumerable<Token> tokens, string text)
        {
            var syntaxTree = new SyntaxTree();
            tokens.Aggregate<Token, SyntaxTreeNode>(null,
                (current, token) => ProcessToken(token, syntaxTree, current, text));

            separatorStack.Clear();

            return syntaxTree;
        }

        private SyntaxTreeNode ProcessToken(Token token, SyntaxTree syntaxTree, SyntaxTreeNode currentNode,
            string text)
        {
            if (syntaxTree.Root == null)
            {
                syntaxTree.Root = new SyntaxTreeNode(token);
                return syntaxTree.Root;
            }

            if (token.IsSeparator &&
                rules.IsSeparatorValid(text, token.Position, !IsEndSeparator(token)))
            {
                return ProcessSeparatorToken(token, syntaxTree, currentNode);
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
            if (IsEndSeparator(token))
            {
                separatorStack.Pop();
                currentNode.IsClosed = true;
                return separatorStack.Count == 0 ? syntaxTree.Root : separatorStack.Peek();
            }

            var separatorNode = new SyntaxTreeNode(token);
            separatorStack.Push(separatorNode);
            currentNode.AddChild(separatorNode);
            return separatorNode;
        }

        private bool IsEndSeparator(Token token)
        {
            return separatorStack.Count > 0 && separatorStack.Peek().Token.Value == token.Value;
        }
    }
}