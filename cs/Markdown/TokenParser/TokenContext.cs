using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public class TokenContext
    {
        public readonly Token Token;
        public readonly List<TokenNode> Children = new();
        public readonly bool IsSplitWord;

        public TokenContext(Token token, bool isSplitWord)
        {
            IsSplitWord = isSplitWord;
            Token = token;
        }

        public string ToText()
        {
            if (Children.Count == 0)
                return Token.Value;
            var stack = new Stack<string>();
            foreach (var child in ((IEnumerable<TokenNode>)Children).Reverse()) PushTokensToStack(stack, child);
            stack.Push(Token.Value);
            return string.Join("", stack);
        }

        private static void PushTokensToStack(Stack<string> stack, TokenNode node)
        {
            foreach (var child in node.Children.Reverse()) PushTokensToStack(stack, child);
            stack.Push(node.Token.Value);
        }
    }
}