using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal class StringHelper
    {
        private readonly TagRules rules;
        private readonly string text;

        private readonly StringBuilder builder = new();
        private readonly Stack<string> singleTagsCloseSymbols = new();
        
        public StringHelper(TagRules rules, string text)
        {
            this.rules = rules;
            this.text = text;
        }

        private static TokenInfo GetCloseToken(TokenSegment segment)
        {
            return new TokenInfo(
                segment.GetBaseTag().End is null ? -1 : segment.EndPosition,
                segment.GetBaseTag().End ?? segment.GetBaseTag().Start, true);
        }
        
        private static TokenInfo GetOpenToken(TokenSegment segment)
        {
            return new TokenInfo(segment.StartPosition, segment.GetBaseTag().Start, false, true);
        }

        private static IEnumerable<TokenInfo> DecomposeIntoTokens(IEnumerable<TokenSegment> tokenSegments)
        {
            var sortedSegments = tokenSegments
                .OrderBy(x => x.StartPosition)
                .Where(x => !x.IsEmpty())
                .ToList();

            return sortedSegments
                .Select(GetOpenToken)
                .Union(sortedSegments.Select(GetCloseToken))
                .Where(x => x.Position != -1)
                .OrderBy(x => x.Position);
        }

        private void CloseAllSingleTokens()
        {
            while (singleTagsCloseSymbols.Any())
                builder.Append(singleTagsCloseSymbols.Pop());
        }
        
        public string ReplaceTokens(IEnumerable<TokenSegment> tokenSegments, ITagTranslator translator)
        {
            var lastTokenEndIndex = 0;

            foreach (var tokenInfo in DecomposeIntoTokens(tokenSegments))
            {
                var (position, token, _, isOpenToken, _, _) = tokenInfo;
                builder.Append(text.Substring(lastTokenEndIndex, position - lastTokenEndIndex));
                
                if (rules.IsInterruptTag(tokenInfo.Tag))
                    CloseAllSingleTokens();
                
                var tag = tokenInfo.Tag;
                var translatedTag = translator.Translate(tag);
                builder.Append(isOpenToken ? translatedTag.Start : translatedTag.End);
                
                lastTokenEndIndex = position + token.Length;
                if (tag.End is null && isOpenToken && translatedTag.End is not null) 
                    singleTagsCloseSymbols.Push(translatedTag.End);
            }

            builder.Append(text[lastTokenEndIndex..]);
            CloseAllSingleTokens();
            
            return builder.ToString();
        }
    }
}