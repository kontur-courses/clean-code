using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDown.TokenParsers
{
    public class ParserGetter
    {
        private readonly Dictionary<TokenType, TokenParser> tokenParsers;
        public HashSet<char> FirstTokenChars;

        public ParserGetter()
        {
            tokenParsers = new Dictionary<TokenType, TokenParser>()
            {
                {TokenType.EM, new EMParser()},
                {TokenType.Strong, new StrongParser()},
            };
            FirstTokenChars = tokenParsers.Values.Select(v => v.OpeningTags.from[0])
                .Concat(tokenParsers.Values.Select(v => v.OpeningTags.to[0])).ToHashSet();
        }

        public List<TokenParser> GetTokenParsers() => tokenParsers.Values.ToList();

        public TokenParser GetParserFromType(TokenType type) => tokenParsers[type];
    }
}