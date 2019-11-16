using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class Tokenizer
    {
        public static IEnumerable<Token> ParseText(string source, Syntax syntax)
        {
            var tokens =
                GetRawTokens(source, syntax)
                    .GetNonEscapedTokens()
                    .RemoveNonPairDelimiters()
                    .MergeAdjacentDelimiters();

            return tokens;
        }

        private static IEnumerable<Token> GetRawTokens(string source, Syntax syntax)
        {
            for (var i = 0; i < source.Length; i++)
                if (syntax.TryGetCharAttribute(source, i, out var type))
                    yield return CreateToken(syntax, type, source, i);
        }


        public static Token CreateToken(Syntax syntax, AttributeType  type, string source, int charPosition)
        {
            var tokenType = type;

            if (tokenType == AttributeType.Emphasis)
                return new PairToken(tokenType, charPosition, syntax.IsClosingDelimiter(source, charPosition));
            if (tokenType == AttributeType.Escape)
                return new Token(tokenType, charPosition);

            throw new Exception("Couldn't create token of given type");
        }

        private static IEnumerable<Token> GetNonEscapedTokens(this IEnumerable<Token> tokens)
        {
            Token previous = null;
            foreach (var token in tokens)
            {
                if (previous == null || previous.Type != AttributeType.Escape || previous.Position != token.Position - 1)
                {
                    yield return token;
                    previous = token;
                }
            }
        }

        private static IEnumerable<Token> RemoveNonPairDelimiters(this IEnumerable<Token> tokens)
        {
            var stack = new Stack<PairToken>();
            var validTokens = new List<Token>();

            foreach (var token in tokens)
                if (token is PairToken pairToken)
                {
                    if (pairToken.IsClosing && stack.Count > 0 && !stack.Peek().IsClosing)
                    {
                        validTokens.Add(stack.Pop());
                        validTokens.Add(pairToken);
                    }
                    else
                    {
                        stack.Push(pairToken);
                    }
                }
                else
                {
                    validTokens.Add(token);
                }

            return validTokens.OrderBy(token => token.Position);
        }

        private static IEnumerable<Token> MergeAdjacentDelimiters(this IEnumerable<Token> tokens)
        {
            var resultTokens = new List<Token>();
            var openingTokens = new Stack<PairToken>();

            PairToken lastOpeningToken = null;
            PairToken lastClosingToken = null;

            foreach (var token in tokens)
            {
                if (token is PairToken pairToken)
                {
                    if (!pairToken.IsClosing)
                    {
                        if (lastOpeningToken == null)
                        {
                            lastOpeningToken = pairToken;
                            continue;
                        }

                        if (lastOpeningToken.Position == pairToken.Position - 1)
                        {
                            openingTokens.Push(lastOpeningToken);
                            openingTokens.Push(pairToken);
                            lastOpeningToken = null;
                        }
                        else
                        {
                            resultTokens.Add(lastOpeningToken);
                            lastOpeningToken = pairToken;
                        }
                    }
                    else
                    {
                        if (lastClosingToken == null)
                        {
                            if (openingTokens.Count > 0)
                                lastClosingToken = pairToken;
                            else
                                resultTokens.Add(pairToken);
                        }
                        else
                        {
                            if (lastClosingToken.Position == pairToken.Position - 1)
                            {
                                openingTokens.Pop();
                                resultTokens.Add(
                                    new PairToken(AttributeType.Strong,
                                        openingTokens.Pop().Position,
                                        false,
                                        2)
                                );
                                resultTokens.Add(
                                    new PairToken(AttributeType.Strong,
                                        lastClosingToken.Position,
                                        true,
                                        2)
                                );
                                lastClosingToken = null;
                            }
                            else
                            {
                                resultTokens.Add(lastClosingToken);
                                lastClosingToken = pairToken;
                            }

                            if (lastOpeningToken != null)
                                resultTokens.Add(lastOpeningToken);
                            lastOpeningToken = null;
                        }
                    }
                }
                else
                {
                    resultTokens.Add(token);
                }
            }

            if (lastOpeningToken != null)
                resultTokens.Add(lastOpeningToken);
            if (lastClosingToken != null)
                resultTokens.Add(lastClosingToken);

            while (openingTokens.Count > 0)
                resultTokens.Add(openingTokens.Pop());

            return resultTokens.OrderBy(token => token.Position);
        }
    }
}