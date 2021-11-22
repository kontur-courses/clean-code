using System;
using System.Collections.Generic;

namespace Markdown
{
    internal class TokenParserConfigurator
    {
        protected List<Token> Tokens = new();

        protected TokenParserConfigurator()
        {
        }

        public static TokenParserConfigurator CreateTokenParser()
        {
            return new TokenParserConfigurator();
        }

        public PreferTokenRulesConfigurator AddToken(Token token)
        {
            Tokens.Add(token);
            return new PreferTokenRulesConfigurator(Tokens);
        }

        public TokenParserConfigurator SetShieldingSymbol(char symbol)
        {
            throw new NotImplementedException();
        }
        
        public ITokenParser Configure()
        {
            var parser = new TokenParser();
            parser.SetTokens(Tokens, null);
            return parser;
        }
    }
}