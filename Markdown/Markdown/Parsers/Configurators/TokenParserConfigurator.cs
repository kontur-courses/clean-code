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

        public TokenParserConfigurator AddTagInterruptToken(Tag tag)
        {
            Config.InterruptTags.Add(tag);
            return this;
        }

        public TokenParserConfigurator SetShieldingSymbol(Tag symbol)
        {
            Config.ShieldingSymbol.SetValue(symbol);
            Config.TagRules.SetShieldedTeg(symbol);
            return this;
        }
        
        public ITokenParser Configure()
        {
            if (Config.ShieldingSymbol.Setted && Config.Tokens.Contains(Config.ShieldingSymbol.GetValue().Start))
                throw new ArgumentException($"shielding symbol can not be {Config.ShieldingSymbol.GetValue().Start}, because it's already added like token");
            
            var trie = new Trie();
            foreach (var token in Config.InterruptTags)
            {
                Config.TagRules.AddTagInterruptTag(token);
                trie.Add(token.Start, token.Start);
            }
            foreach (var token in Config.Tokens)
                trie.Add(token, token);
            if (Config.ShieldingSymbol.Setted)
                trie.Add(Config.ShieldingSymbol.GetValue().Start, Config.ShieldingSymbol.GetValue().Start);
            trie.Build();
            
            return new TokenParser(trie, Config.TagRules, Config.ShieldingSymbol.Setted ? Config.ShieldingSymbol.GetValue().Start : null);
        }
    }
}