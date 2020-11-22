﻿using System.Collections.Generic;

namespace Markdown
{
    public class TextParser : ITextParser
    {
        private readonly IEnumerable<ITokenReader> readers;

        public TextParser(IEnumerable<ITokenReader> readers)
        {
            this.readers = readers;
        }

        public IEnumerable<IToken> GetTokens(string text)
        {
            return GetTokens(text, text);
        }

        private IEnumerable<IToken> GetTokens(string text, string context)
        {
            var tokens = new List<IToken>();

            for (var i = 0; i < text.Length; ++i)
                foreach (var reader in readers)
                {
                    if (!reader.TryReadToken(text, context, i, out var token))
                        continue;

                    if (token!.CanHaveChildTokens)
                    {
                        var newContext = context[token.Position..(token.EndPosition + 1)];
                        var childTokens = GetTokens(token!.Value, newContext);

                        token!.ChildTokens.AddRange(childTokens);
                    }

                    tokens.Add(token);
                    i = token.EndPosition;
                    break;
                }

            return tokens;
        }
    }
}