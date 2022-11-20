using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Markdown;

namespace Markdown.Tokens
{
    public static class UnpairedTokensFixer
    {
        public static IEnumerable<Token> RemoveSingleTokens(this IEnumerable<Token> tokens, string markdownString)
        {
            var unknownTags = new Stack<Token>();
            var stackTokens = new Stack<Token>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Header)
                {
                    yield return token;
                    continue;
                }

                if (token.Type == TokenType.Default)
                {
                    foreach (var textToken in GetTextToken(markdownString, token, unknownTags))
                        yield return textToken;
                    yield return token;
                    continue;
                }

                foreach (var tokenMd in EnumerableTokens(markdownString, token, stackTokens, unknownTags))
                    yield return tokenMd;
            }

            foreach (var singleToken in ToStringTokensWithoutPair(stackTokens, unknownTags))
                yield return singleToken;

        }

        private static IEnumerable<Token> ToStringTokensWithoutPair(Stack<Token> stackTokens, Stack<Token> unknownTags)
        {
            foreach (var token in stackTokens.Concat(unknownTags))
            {
                token.ToDefault();
                yield return token;
            }
        }

        private static IEnumerable<Token> EnumerableTokens(string md, Token token, Stack<Token> stackTokens,
            Stack<Token> unknownTags)
        {
            if (token.Element == TokenElement.Open)
                stackTokens.Push(token);
            else if (token.Element == TokenElement.Close)
                foreach (var closeTag in stackTokens.CloseTags(unknownTags, token))
                    yield return closeTag;
            else if (token.Element == TokenElement.Unknown)
                foreach (var unknownTag in unknownTags.UnknownTags(md, stackTokens, token))
                    yield return unknownTag;
            else
                yield return token;
        }

        private static IEnumerable<Token> GetTextToken(string md, Token token, Stack<Token> unknownTags)
        {
            var mdStringBetweenTags = md.Substring(token.Position, token.Length);
            if (!mdStringBetweenTags.Contains(' '))
                yield break;
            foreach (var unknownTag in unknownTags)
            {
                unknownTag.ToDefault();
                yield return unknownTag;
            }
        }

        private static IEnumerable<Token> CloseTags(this Stack<Token> stackTags, Stack<Token> unknownStackTags,
            Token token)
        {
            if (stackTags.Count <= 0)
            {
                if (unknownStackTags.Count > 0 && unknownStackTags.Peek().Type == token.Type)
                {
                    unknownStackTags.Peek().Element = TokenElement.Open;
                    yield return unknownStackTags.Pop();
                    yield return token;
                    yield break;
                }

                token.ToDefault();
                yield return token;
                yield break;
            }

            var last = stackTags.Pop();
            if (last.Type != token.Type)
            {
                last.ToDefault();
                token.ToDefault();
            }

            yield return last;
            yield return token;
        }


        private static IEnumerable<Token> UnknownTags(this Stack<Token> undefinedStackTags, string md,
            Stack<Token> tagsStack, Token token)
        {
            if (undefinedStackTags.Count == 0)
            {
                if (tagsStack.Count > 0 && tagsStack.Peek().Type == token.Type)
                {
                    if (!md.Substring(tagsStack.Peek().End + 1,
                            token.Position - tagsStack.Peek().End - 1).Contains(' '))
                    {
                        token.Element = TokenElement.Close;
                        yield return tagsStack.Pop();
                        yield return token;
                    }
                    else
                        undefinedStackTags.Push(token);
                }
                else
                    undefinedStackTags.Push(token);
            }
            else if (undefinedStackTags.Peek().Type == token.Type)
            {
                undefinedStackTags.Peek().Element = TokenElement.Open;
                token.Element = TokenElement.Close;
                yield return undefinedStackTags.Pop();
                yield return token;
            }
            else
                undefinedStackTags.Push(token);
        }
    }
}
