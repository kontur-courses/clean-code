using System;
using System.Collections.Generic;
using Markdown.Rules;
using Markdown.SyntaxAnalysis.SyntaxTreeRealization;
using Markdown.Tokenization;

namespace Markdown.SyntaxAnalysis.SyntaxTreeBuilders
{
    public class MarkdownSyntaxTreeBuilder : ISyntaxTreeBuilder
    {
        private readonly IRules rules;
        private readonly Stack<SyntaxTreeNode> separatorNodesStack;
        private SyntaxTreeNode currentNode;
        private SyntaxTree currentSyntaxTree;

        public MarkdownSyntaxTreeBuilder(IRules rules)
        {
            this.rules = rules;
            separatorNodesStack = new Stack<SyntaxTreeNode>();
        }

        public SyntaxTree BuildSyntaxTree(IEnumerable<Token> tokens, string text)
        {
            var root = new SyntaxTreeNode(new Token(0, string.Empty, false));
            currentSyntaxTree = new SyntaxTree(root);
            currentNode = root;

            foreach (var token in tokens)
            {
                ProcessToken(token, text);
            }

            MarkUnclosedSeparators();
            separatorNodesStack.Clear();

            return currentSyntaxTree;
        }

        private void ProcessToken(Token token, string text)
        {
            if (IsValidSeparator(token, text))
            {
                ProcessSeparatorToken(token);
            }
            else
            {
                if (token.IsSeparator)
                {
                    token.IsSeparator = false;
                }
                currentNode.AddChild(new SyntaxTreeNode(token));
            }
        }

        private void ProcessSeparatorToken(Token token)
        {
            if (token.Value.StartsWith("\\"))
            {
                ProcessCommentedSeparator(token);
            }
            else if(rules.IsSeparatorPaired(token.Value))
            {
                ProcessPairedSeparatorToken(token);
            }
            else
            {
                throw new NotImplementedException($"separator not supported {token.Value}");
            }
        }

        private void ProcessCommentedSeparator(Token token)
        {
            var newToken = new Token(token.Position + 1, token.Value.Substring(1), false);
            var tokenNode = new SyntaxTreeNode(newToken);
            currentNode.AddChild(tokenNode);
        }

        private void ProcessPairedSeparatorToken(Token token)
        {
            var separatorNode = new SyntaxTreeNode(token);
            currentNode.AddChild(separatorNode);

            if (IsEndSeparator(token))
            {
                separatorNodesStack.Pop();
                currentNode = separatorNodesStack.Count == 0 ? currentSyntaxTree.Root : separatorNodesStack.Peek();
            }
            else if (rules.IsSeparatorOpening(token.Value))
            {
                separatorNodesStack.Push(separatorNode);
                currentNode = separatorNode;
            }
            else
            {
                token.IsSeparator = false;
            }
        }

        private bool IsEndSeparator(Token token)
        {
            return separatorNodesStack.Count > 0 &&
                   rules.IsSeparatorPairedFor(separatorNodesStack.Peek().Token.Value,
                       token.Value);
        }

        private bool IsValidSeparator(Token token, string text)
        {
            if (!token.IsSeparator)
                return false;

            var isFirst = !IsEndSeparator(token);
            var hasParentSeparator =
                separatorNodesStack.Count > 0 && separatorNodesStack.Peek().Token.Value != token.Value;

            return hasParentSeparator
                ? rules.IsSeparatorValid(text, token.Position, isFirst, token.Value.Length,
                    separatorNodesStack.Peek().Token.Value)
                : rules.IsSeparatorValid(text, token.Position, isFirst, token.Value.Length);
        }

        private void MarkUnclosedSeparators()
        {
            foreach (var syntaxTreeNode in separatorNodesStack)
            {
                syntaxTreeNode.Token.IsSeparator = false;
            }
        }
    }
}