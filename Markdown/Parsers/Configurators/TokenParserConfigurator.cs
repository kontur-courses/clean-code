using System;

namespace Markdown
{
    internal class TokenParserConfigurator
    {
        protected TokenParserConfig Config;

        protected TokenParserConfigurator()
        {
            Config = new TokenParserConfig();
        }
        
        protected internal TokenParserConfigurator(TokenParserConfig config)
        {
            Config = config;
        }

        public static TokenParserConfigurator CreateTokenParser()
        {
            return new TokenParserConfigurator();
        }

        public PreferTokenRulesConfigurator AddToken(Tag token)
        {
            Config.Tokens.Add(token.Start);
            Config.LastAddedToken = token;
            return new PreferTokenRulesConfigurator(Config);
        }

        public TokenParserConfigurator SetShieldingSymbol(char symbol)
        {
            if (Config.ShieldingSymbol.Setted) throw new ArgumentException("shielding symbol already setted");
            Config.ShieldingSymbol = (symbol, true);
            return this;
        }
        
        public ITokenParser Configure()
        {
            var parser = new TokenParser();
            parser.SetTokens(Config.Tokens, null);
            return parser;
        }
    }
}