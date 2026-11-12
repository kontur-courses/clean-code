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

            if (TryOpenTag(token, stack)) return;

            AddText(token, stack, root);
        }

        private static bool TryOpenTag(Token token, Stack<StackItem> stack)
        {
            if (!PairRules.CanOpen(token, stack.Select(x => x.Token)))
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
                if (PairRules.IsIntersection(token, stack.Select(x => x.Token)))
                {
                    HandleIntersection(token, stack, root);
                    return true;
                }
                return false;
            }

            if (!PairRules.CanClose(topItem.Token, topItem.Children, token))
            {
                RemoveTopAsText(stack, root);
                AddText(token, stack, root);
                return true;
            }

            stack.Pop();
            AddNode(CreateNode(topItem), stack, root);
            return true;
        }

        private static void HandleIntersection(Token token, Stack<StackItem> stack, List<INode> root)
        {
            token.IsDisabledFromIntersection = true;

            while (stack.Count > 0)
            {
                var top = stack.Peek();
                bool isTargetType = top.Token.Type == token.Type;

                top.Token.IsDisabledFromIntersection = true;
                RemoveTopAsText(stack, root);

                if (isTargetType) break;
            }
        }

        private static void RemoveTopAsText(Stack<StackItem> stack, List<INode> root)
        {
            var item = stack.Pop();

            AddText(item.Token, stack, root);

            foreach (var child in item.Children)
                AddNode(child, stack, root);
        }

        private static void AddText(Token token, Stack<StackItem> stack, List<INode> root) =>
            AddNode(new TextNode(token.Value), stack, root);

        private static void AddNode(INode node, Stack<StackItem> stack, List<INode> root)
        {
            if (stack.Count == 0) root.Add(node);
            else stack.Peek().Children.Add(node);
        }

        private static INode CreateNode(StackItem item) => 
            item.Token.Type == TokenType.Em
                ? new EmNode(item.Children)
                : new StrongNode(item.Children);

        private static void FlushUnclosedTags(Stack<StackItem> stack, List<INode> root)
        {
            while (stack.Count > 0)
            {
                RemoveTopAsText(stack, root);
            }
        }
    }
}