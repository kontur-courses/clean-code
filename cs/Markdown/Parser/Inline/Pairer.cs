using System.Collections.Generic;
using System.Linq;
using Markdown.Entity;
using Markdown.Parser.Inline.Tokens;

namespace Markdown.Parser.Inline
{
    public static class Pairer
    {
        private class StackItem
        {
            public Token Token { get; }
            public List<INode> Children { get; } = new();
            public StackItem(Token token) => Token = token;
        }

        public static List<INode> Pairing(List<Token> tokens)
        {
            var root = new List<INode>();
            var stack = new Stack<StackItem>();

            foreach (var token in tokens)
            {
                ProcessToken(token, stack, root);
            }

            FlushUnclosedTags(stack, root);
            return root;
        }

        private static void ProcessToken(Token token, Stack<StackItem> stack, List<INode> root)
        {
            if (token.Type == TokenType.Text || token.IsDisabledFromIntersection)
            {
                AddText(token, stack, root);
                return;
            }

            if (TryCloseTag(token, stack, root)) return;

            if (!token.IsDisabledFromIntersection && TryOpenTag(token, stack)) return;

            AddText(token, stack, root);
        }

        private static bool TryOpenTag(Token token, Stack<StackItem> stack)
        {
            if (!token.CanOpen) return false;

            if (token.Type == TokenType.Strong && stack.Any(x => x.Token.Type == TokenType.Em))
                return false;

            stack.Push(new StackItem(token));
            return true;
        }

        private static bool TryCloseTag(Token token, Stack<StackItem> stack, List<INode> root)
        {
            if (!token.CanClose || stack.Count == 0) return false;

            var topItem = stack.Peek();

            if (topItem.Token.Type != token.Type)
            {
                if (HandleIntersection(token, topItem, stack, root))
                    return false;

                return false;
            }

            if (topItem.Children.Count == 0)
            {
                RemoveTopAsText(stack, root);
                return false;
            }

            if (topItem.Token.IsInsideWord && token.IsInsideWord)
            {
                if (HasWhitespaceInside(topItem.Children))
                {
                    RemoveTopAsText(stack, root);
                    return false;
                }
            }

            stack.Pop();
            var closedNode = CreateNode(topItem);
            AddNode(closedNode, stack, root);
            return true;
        }

        private static bool HandleIntersection(Token token, StackItem topItem, Stack<StackItem> stack, List<INode> root)
        {
            var intersectingItem = stack.FirstOrDefault(x => x.Token.Type == token.Type);
            if (intersectingItem == null) return false;

            intersectingItem.Token.IsDisabledFromIntersection = true;
            token.IsDisabledFromIntersection = true;
            topItem.Token.IsDisabledFromIntersection = true;

            stack.Pop();
            AddNode(new TextNode(topItem.Token.Value), stack, root);
            foreach (var child in topItem.Children)
            {
                AddNode(child, stack, root);
            }

            return true;
        }

        private static bool HasWhitespaceInside(List<INode> children)
        {
            return children.Any(c => c is TextNode t && t.Text.Any(char.IsWhiteSpace));
        }

        private static void RemoveTopAsText(Stack<StackItem> stack, List<INode> root)
        {
            var item = stack.Pop();
            AddText(item.Token, stack, root);

            foreach(var child in item.Children)
            {
                AddNode(child, stack, root);
            }
        }

        private static void AddText(Token token, Stack<StackItem> stack, List<INode> root)
        {
            AddNode(new TextNode(token.Value), stack, root);
        }

        private static void AddNode(INode node, Stack<StackItem> stack, List<INode> root)
        {
            if (stack.Count == 0) root.Add(node);
            else stack.Peek().Children.Add(node);
        }

        private static INode CreateNode(StackItem item)
        {
            return item.Token.Type == TokenType.Em
                ? new EmNode(item.Children)
                : new StrongNode(item.Children);
        }

        private static void FlushUnclosedTags(Stack<StackItem> stack, List<INode> root)
        {
            while (stack.Count > 0)
            {
                var item = stack.Pop();
                AddText(item.Token, stack, root);

                foreach (var child in item.Children)
                {
                    AddNode(child, stack, root);
                }
            }
        }
    }
}