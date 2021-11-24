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

        public TokenParserConfigurator AddTagInterruptToken(Tag token)
        {
            if (token.End is not null) throw new ArgumentException();
            Config.InterruptTokens.Add(token.Start);
            return this;
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
            
            var trie = new Trie();
            foreach (var token in Config.InterruptTokens)
            {
                Config.TagRules.AddTagInterruptTag(Tag.GetTagByChars(token));
                trie.Add(token, token);
            }
            foreach (var token in Config.Tokens)
                trie.Add(token, token);
            if (Config.ShieldingSymbol.Setted)
                trie.Add(Config.ShieldingSymbol.Symbol.Start, Config.ShieldingSymbol.Symbol.Start);
            trie.Build();
            
            return new TokenParser(trie, Config.TagRules, Config.ShieldingSymbol.Setted ? Config.ShieldingSymbol.Symbol.Start : null);
        }
    }
}