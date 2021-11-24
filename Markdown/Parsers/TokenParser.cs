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

        public (SegmentsCollection, SegmentsCollection) IgnoreSegmentsThatDoNotMatchRules(SegmentsCollection first, SegmentsCollection second)
        {
            var firstSorted = first.GetSortedSegments().Where(x => rules.DoesMatchInFrontRule(x)).ToList();
            var secondSorted = second.GetSortedSegments().Where(x => rules.DoesMatchInFrontRule(x)).ToList();
            
            var firstSegments = firstSorted
                .Where(x => !x.InTextSegment || secondSorted.All(y => rules.DoesMatchContainRule(x, y)))
                .Where(x => secondSorted.All(y => rules.DoesMatchIntersectingRule(x, y))).ToList();
            
            var secondSegment = secondSorted
                .Where(x => !x.InTextSegment || firstSorted.All(y => rules.DoesMatchContainRule(x, y)))
                .Where(x => firstSorted.All(y => rules.DoesMatchIntersectingRule(x, y))).ToList();

            return (
                new SegmentsCollection(firstSegments.Where(x => secondSegment.All(y => rules.DoesMatchNestingRule(y, x)))), 
                new SegmentsCollection(secondSegment.Where(x => firstSegments.All(y => rules.DoesMatchNestingRule(y, x))))
                );
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
                var tag = Tag.GetTagByChars(token.ToString());
                result.Append(text.Substring(lastTokenEndIndex, index - lastTokenEndIndex));
                
                var translatedTag = translator.Translate(tag);
                result.Append(start ? translatedTag.Start : translatedTag.End);

                lastTokenEndIndex = index + token.Length;
                if (tag.End is null && start && translatedTag.End is not null) 
                    singleTagsCloseSymbols.Push(translatedTag.End.ToString());
            }

            result.Append(text[lastTokenEndIndex..]);
            while (singleTagsCloseSymbols.Any())
                result.Append(singleTagsCloseSymbols.Pop());
            
            return result.ToString();
        }

        public TokenInfoCollection FindAllTokens(string paragraph)
        {
            var tokenInfos = new Dictionary<int, TokenInfo>();
            var currentSearchStartIndex = 0;
            var lastIndex = 0;
            TokenInfo lastShieldToken = null;

            foreach (var (token, index) in trie.Find(paragraph))
            {
                if (token is null || currentSearchStartIndex > index && lastIndex != index) continue;
                if (shieldingSymbol is not null 
                    && lastShieldToken is not null 
                    && lastShieldToken.Position + shieldingSymbol.Length == index)
                {
                    lastShieldToken = null;
                    continue;
                }

                if (lastShieldToken is not null)
                {
                    lastShieldToken.Valid = false;
                }

                var closeValid = index > 0 && !char.IsWhiteSpace(paragraph[index - 1]);
                var openValid = index < paragraph.Length - token.Length 
                                && !char.IsWhiteSpace(paragraph[index + token.Length]);

                var tokenInfo = new TokenInfo(
                    index,
                    new Token(token), closeValid, openValid,
                    closeValid && openValid,
                    closeValid || openValid || shieldingSymbol is not null && shieldingSymbol == token
                );
                
                tokenInfos[lastIndex = index] = tokenInfo;
                currentSearchStartIndex = index + token.Length;

                if (token == shieldingSymbol)
                    lastShieldToken = tokenInfo;
            }
            
            return new TokenInfoCollection(tokenInfos.Select(x => x.Value));
        }
    }
}