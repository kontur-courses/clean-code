using System;
using AhoCorasick;

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

        public TokenParserConfigurator SetShieldingSymbol(Tag symbol)
        {
            if (Config.ShieldingSymbol.Setted) throw new ArgumentException("shielding symbol already setted");
            Config.ShieldingSymbol = (symbol, true);
            return this;
        }
        
        public ITokenParser Configure()
        {
            if (Config.ShieldingSymbol.Setted && Config.Tokens.Contains(Config.ShieldingSymbol.Symbol.Start))
                throw new ArgumentException($"shielding symbol can not be {Config.ShieldingSymbol.Symbol.Start}, because it's already added like token");
            
            var trie = new Trie<Token>();
            
            foreach (var token in Config.Tokens)
                trie.Add(token.ToString(), token);
            if (Config.ShieldingSymbol.Setted)
                trie.Add(Config.ShieldingSymbol.Symbol.Start.ToString(), Config.ShieldingSymbol.Symbol.Start);
            trie.Build();
            
            return new TokenParser(trie, Config.TagRules, Config.ShieldingSymbol.Setted ? Config.ShieldingSymbol.Symbol.Start.ToString() : null);
        }
    }
}