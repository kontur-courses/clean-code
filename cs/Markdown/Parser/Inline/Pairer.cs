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
            public PairToken Token { get; }
            public List<INode> Children { get; } = new();

            public StackItem(PairToken token) => Token = token;
        }

        public static List<INode> Pairing(List<Token> tokens)
        {
            var root = new List<INode>();
            var stack = new Stack<StackItem>();

            foreach (var token in tokens)
            {
                switch (token)
                {
                    case TextToken textToken:
                        AddNode(new TextNode(textToken.Text), stack, root);
                        break;

                    case LinkToken linkToken:
                        var children = Pairing(Tokenizer.Tokenize(linkToken.Text));
                        AddNode(new LinkNode(linkToken.Url, children), stack, root);
                        break;

                    case PairToken pairToken:
                        ProcessPairToken(pairToken, stack, root);
                        break;
                }
            }

            FlushUnclosedTags(stack, root);
            return root;
        }

        private static void ProcessPairToken(PairToken token, Stack<StackItem> stack, List<INode> root)
        {
            string textValue = GetTokenText(token);

            if (token.IsDisabledFromIntersection)
            {
                AddNode(new TextNode(textValue), stack, root);
                return;
            }

            if (TryCloseTag(token, stack, root)) return;
            if (TryOpenTag(token, stack)) return;

            AddNode(new TextNode(textValue), stack, root);
        }

        private static bool TryOpenTag(PairToken token, Stack<StackItem> stack)
        {
            if (!PairRules.CanOpen(token, stack.Select(x => x.Token)))
                return false;

            stack.Push(new StackItem(token));
            return true;
        }

        private static bool TryCloseTag(PairToken token, Stack<StackItem> stack, List<INode> root)
        {
            if (!token.CanClose || stack.Count == 0) return false;

            var topItem = stack.Peek();

            if (topItem.Token.GetType() != token.GetType())
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
                AddNode(new TextNode(GetTokenText(token)), stack, root);
                return true;
            }

            stack.Pop();
            AddNode(CreateNode(topItem), stack, root);
            return true;
        }

        private static void HandleIntersection(PairToken token, Stack<StackItem> stack, List<INode> root)
        {
            token.IsDisabledFromIntersection = true;

            while (stack.Count > 0)
            {
                var top = stack.Peek();
                bool isTargetType = top.Token.GetType() == token.GetType();

                top.Token.IsDisabledFromIntersection = true;
                RemoveTopAsText(stack, root);

                if (isTargetType) break;
            }
        }

        private static void RemoveTopAsText(Stack<StackItem> stack, List<INode> root)
        {
            var item = stack.Pop();

            AddNode(new TextNode(GetTokenText(item.Token)), stack, root);

            foreach (var child in item.Children)
                AddNode(child, stack, root);
        }

        private static void AddNode(INode node, Stack<StackItem> stack, List<INode> root)
        {
            if (stack.Count == 0) root.Add(node);
            else stack.Peek().Children.Add(node);
        }

        private static INode CreateNode(StackItem item)
        {
            return item.Token switch
            {
                EmToken => new EmNode(item.Children),
                StrongToken => new StrongNode(item.Children),
                _ => throw new System.NotImplementedException($"Unknown token type: {item.Token.GetType()}")
            };
        }

        private static void FlushUnclosedTags(Stack<StackItem> stack, List<INode> root)
        {
            while (stack.Count > 0)
            {
                RemoveTopAsText(stack, root);
            }
        }

        private static string GetTokenText(PairToken token) => token is EmToken ? "_" : "__";
    }
}