using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public static class TypedTokenExtensions
    {
        public static List<TypedToken> RemoveEscaped(this List<TypedToken> tokens)
        {
            var count = tokens.Count;

            var indexesToRemove = new List<int>();

            for (var i = 1; i < count; i++)
            {
                if (tokens[i - 1].Type != TokenType.Escape)
                    continue;

                if (tokens[i].Type == TokenType.Text)
                {
                    tokens[i - 1].SwitchToTextToken();
                    continue;
                }

                indexesToRemove.Add(i - 1);

                tokens[i].SwitchToTextToken();
            }

            for (var i = indexesToRemove.Count - 1; i >= 0; i--)
                tokens.RemoveAt(indexesToRemove[i]);

            return tokens;
        }

        public static List<TypedToken> RemoveUnpaired(this List<TypedToken> tokens)
        {
            var tagTokensStack = new Stack<TypedToken>();

            foreach (var token in tokens)
            {
                if (token.Type != TokenType.Tag)
                    continue;

                if (!tagTokensStack.Any())
                {
                    tagTokensStack.Push(token);
                    continue;
                }

                var previousTagToken = tagTokensStack.Peek();

                if (previousTagToken.TagType == token.TagType && previousTagToken.Order == SubTagOrder.Opening)
                    tagTokensStack.Pop();
                else
                    tagTokensStack.Push(token);
            }

            foreach (var unpairedTagToken in tagTokensStack)
                unpairedTagToken.SwitchToTextToken();

            return tokens;
        }
    }
}
