using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public static class TypedTokenListExtensions
    {
        public static List<TypedToken> RemoveEscapedTags(this List<TypedToken> tokens)
        {
            var indexesToRemove = new List<int>();

            for (var i = 1; i < tokens.Count; i++)
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

        public static List<TypedToken> SwitchToTextUnpairedTags(this List<TypedToken> tokens)
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

        public static List<TypedToken> SwitchToTextTagsWithInvalidContentBetween(this List<TypedToken> tokens, string text)
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

                if (previousTagToken.TagType != token.TagType)
                {
                    tagTokensStack.Push(token);
                    continue;
                }

                if (!IsTagAroundEmptyString(previousTagToken, token) &&
                    !IsTagTokensInsideDifferentWords(text, previousTagToken, token))
                {
                    tagTokensStack.Pop();
                    continue;
                }

                tagTokensStack.Pop().SwitchToTextToken();
                token.SwitchToTextToken();
            }

            return tokens;
        }

        public static List<TypedToken> SwitchTextTagsWithInvalidNesting(this List<TypedToken> tokens, TagStorage tagStorage)
        {
            var tagTokensStack = new Stack<TypedToken>();

            for (var i = 0; i < tokens.Count - 1; i++)
            {
                var token = tokens[i];

                if (tokens[i].Type != TokenType.Tag)
                    continue;

                if (!tagTokensStack.Any())
                {
                    tagTokensStack.Push(token);
                    continue;
                }

                var previousTagToken = tagTokensStack.Peek();

                if (previousTagToken.TagType == token.TagType && previousTagToken.Order == SubTagOrder.Opening)
                {
                    tagTokensStack.Pop();
                    continue;
                }

                if (tagStorage.IsForbiddenTagNesting(previousTagToken.TagType, token.TagType))
                {
                    token.SwitchToTextToken();
                    continue;
                }

                tagTokensStack.Push(token);
            }

            return tokens;
        }

        private static bool IsTagAroundEmptyString(TypedToken previous, TypedToken next)
        {
            if (previous.Order == SubTagOrder.Opening && next.Order == SubTagOrder.Closing)
                return previous.End + 1 == next.Start;

            return false;
        }

        private static bool IsTagTokensInsideDifferentWords(string text, TypedToken previous, TypedToken next)
        {
            if (IsTagBeforeWord(text, previous) && IsTagAfterWord(text, next))
                return false;

            for (var i = previous.End + 1; i < next.Start; i++)
                if (char.IsWhiteSpace(text[i]))
                    return true;

            return false;
        }

        private static bool IsTagBeforeWord(string text, TypedToken token)
        {
            var separators = new HashSet<char> { '\n', ' ', '*', '_', ':' };

            return token.Start == 0 || separators.Contains(text[token.Start - 1]);
        }

        private static bool IsTagAfterWord(string text, TypedToken token)
        {
            var separators = new HashSet<char> { '\r', '\n', ' ', '*', '_' };

            return token.End >= text.Length - 1 || separators.Contains(text[token.End + 1]);
        }
    }
}