using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var result = new StringBuilder();
            var sortedSegments = tokenSegments.GetSortedSegments().Where(x => !x.IsEmpty()).ToList();
            var sortedTokens = sortedSegments
                .Select(x => new TokenInfo(x.StartPosition, x.GetBaseTag().Start, false, true, false, false))
                .Union(sortedSegments
                    .Select(x => new TokenInfo(x.GetBaseTag().End is null ? -1 : x.EndPosition, x.GetBaseTag().End ?? x.GetBaseTag().Start, true, false, false, false)))
                .OrderBy(x => x.Position)
                .Where(x => x.Position != -1);
            var lastTokenEndIndex = 0;
            var singleTagsCloseSymbols = new Stack<string>();

            foreach (var (index, token, _, start, _, _) in sortedTokens)
            {
                result.Append(text.Substring(lastTokenEndIndex, index - lastTokenEndIndex));
                if (rules.IsInterruptTag(Tag.GetTagByChars(token)))
                    while (singleTagsCloseSymbols.Any())
                        result.Append(singleTagsCloseSymbols.Pop());
                
                var tag = Tag.GetTagByChars(token);
                var translatedTag = translator.Translate(tag);
                result.Append(start ? translatedTag.Start : translatedTag.End);
                
                lastTokenEndIndex = index + token.Length;
                if (tag.End is null && start && translatedTag.End is not null) 
                    singleTagsCloseSymbols.Push(translatedTag.End);
                
            }

            result.Append(text[lastTokenEndIndex..]);
            while (singleTagsCloseSymbols.Any())
                result.Append(singleTagsCloseSymbols.Pop());
            
            return result.ToString();
        }

        public TokenInfoCollection FindAllTokens(string text)
        {
            return new MdTokenFinder(trie, rules, shieldingSymbol).FindAllTokens(text);
        }
    }
}