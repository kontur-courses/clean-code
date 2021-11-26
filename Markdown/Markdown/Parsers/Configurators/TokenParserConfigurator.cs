using System;
using System.Collections.Generic;
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

        public PreferTokenRulesConfigurator AddToken(Tag tag)
        {
            Config.Tokens.Add(tag.Start);
            Config.LastAddedToken = tag;
            return new PreferTokenRulesConfigurator(Config);
        }

        public TokenParserConfigurator AddTagsShell(Tag shellTag, params Tag[] tagsToShell)
        {
            if (tagsToShell.Length == 0) throw new ArgumentException("There are no tags to shell");
            if (!Config.Shells.ContainsKey(shellTag)) Config.Shells[shellTag] = new List<Tag>();
            foreach (var tagToShell in tagsToShell)
                Config.Shells[shellTag].Add(tagToShell);
            return this;
        }

        public TokenParserConfigurator AddTagInterruptToken(Tag tag)
        {
            Config.InterruptTags.Add(tag);
            return this;
        }
        
        public TokenParserConfigurator AddNewLineToken(Tag tag)
        {
            Config.NewLineTags.Add(tag);
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

            foreach (var (shell, shelleds) in Config.Shells)
            {
                foreach (var shelled in shelleds)
                {
                    Config.TagRules.AddShellReference(shelled, shell);
                }
            }
            
            var trie = new Trie();
            foreach (var token in Config.NewLineTags)
            {
                Config.TagRules.AddNewLineTag(token);
                trie.Add(token.Start, token.Start);
            }
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