using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDown.TokenParsers
{
    public class ParserGetter
    {

        private Dictionary<TokenType, TokenParser> tokenParsers;

        public ParserGetter()
        {
            tokenParsers = new Dictionary<TokenType, TokenParser>()
            {
                {TokenType.EM, new EMParser() },
                {TokenType.Strong, new StrongParser() },
            };
        }

        public List<TokenParser> GetTokenParsers() => tokenParsers.Values.ToList();

        public TokenParser GetParserFromType(TokenType type) => tokenParsers[type];

    }
}