using System.Collections.Generic;
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
            MdExceptionHelper.ThrowArgumentNullExceptionIf(
                new ExceptionCheckObject(nameof(trie), trie),
                new ExceptionCheckObject(nameof(rules), rules),
                new ExceptionCheckObject(nameof(shieldingSymbol), shieldingSymbol));
            this.shieldingSymbol = shieldingSymbol;
            this.trie = trie;
            this.rules = rules;
        }

        public TagRules GetRules()
        {
            return rules;
        }

        public string ReplaceTokens(string text, IEnumerable<TokenSegment> tokenSegments, ITagTranslator translator)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(
                new ExceptionCheckObject(nameof(text), text),
                new ExceptionCheckObject(nameof(tokenSegments), tokenSegments),
                new ExceptionCheckObject(nameof(translator), translator));
            return new StringHelper(rules, text).ReplaceTokens(tokenSegments, translator);
        }

        public TokenInfoCollection FindAllTokens(string text)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(text), text));
            return new MdTokenFinder(trie, rules, shieldingSymbol).FindAllTokens(text);
        }
    }
}