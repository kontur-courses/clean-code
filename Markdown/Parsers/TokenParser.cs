using AhoCorasick;

namespace Markdown
{
    internal class TokenParser : ITokenParser
    {
        private readonly Trie trie;
        private readonly TagRules rules;
        private readonly string shieldingSymbol;

        internal TokenParser(Trie trie, TagRules rules, string shieldingSymbol)
        {
            this.shieldingSymbol = shieldingSymbol;
            this.trie = trie;
            this.rules = rules;
        }

        public TagRules GetRules()
        {
            return rules;
        }

        public string ReplaceTokens(string text, SegmentsCollection tokenSegments, ITagTranslator translator)
        {
            return new StringHelper(rules, text).ReplaceTokens(tokenSegments, translator);
        }

        public TokenInfoCollection FindAllTokens(string text)
        {
            return new MdTokenFinder(trie, rules, shieldingSymbol).FindAllTokens(text);
        }
    }
}