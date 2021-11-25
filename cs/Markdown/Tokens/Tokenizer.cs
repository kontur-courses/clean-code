using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TagStore;

namespace Markdown.Tokens
{
    public class Tokenizer : ITokenizer
    {
        private static readonly HashSet<string> Escaping = new() { "\\" };
        private readonly ITagStore store;

        public Tokenizer(ITagStore store)
        {
            this.store = store;
        }

        public IEnumerable<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();

            var tagTokens = text.FindAll(store.GetTagValues().Concat(Escaping))
                .OrderBy(s => s.Start).ToArray();

            tokens.AddTextBefore(tagTokens[0]);

            for (var i = 0; i < tagTokens.Length; i++)
            {
                if (i > 0)
                {
                    tokens.AddTextBetween(tagTokens[i - 1], tagTokens[i]);
                }

                if (text[tagTokens[i].Start] == '\\')
                {
                    tokens.Add(new Token(TokenType.Escape, tagTokens[i].Start, 1));
                    continue;
                }

                var tagType = store.GetTagType(text, tagTokens[i].Start, tagTokens[i].Length);
                var tagRole = store.GetTagRole(text, tagTokens[i].Start, tagTokens[i].Length);
                var token = new Token(TokenType.Tag, tagType, tagRole, tagTokens[i].Start, tagTokens[i].Length);
                tokens.Add(token);
            }

            tokens.AddTextAfter(tagTokens[^1], text.Length - 1);

            return tokens.RemoveEscaping().RemoveUnpaired(text).OrderBy(t => t.Start);
        }
    }
}