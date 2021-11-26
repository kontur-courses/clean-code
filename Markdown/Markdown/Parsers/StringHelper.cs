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
        private readonly Stack<TokenInfo> singleTagsCloseSymbols = new();
        
        public StringHelper(TagRules rules, string text)
        {
            this.rules = rules;
            this.text = text;
        }

        private static TokenInfo GetCloseToken(TokenSegment segment)
        {
            return new TokenInfo(
                segment.GetBaseTag().End is null ? -1 : segment.EndPosition,
                segment.GetBaseTag().End ?? segment.GetBaseTag().Start, true
                ) {ShellStatus = segment.EndTokenInfo?.ShellStatus};
        }
        
        private static TokenInfo GetOpenToken(TokenSegment segment)
        {
            return new TokenInfo(segment.StartPosition, segment.GetBaseTag().Start, false, true) {ShellStatus = segment.StartTokenInfo!.ShellStatus};
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
            {
                var singleToken = singleTagsCloseSymbols.Pop();
                builder.Append(singleToken.Token);
                if (singleToken.ShellStatus?.IsEndShellNeed ?? false)
                    builder.Append(singleToken.ShellStatus.ShellTag.End);
            }
        }
        
        public string ReplaceTokens(IEnumerable<TokenSegment> tokenSegments, ITagTranslator translator)
        {
            var lastTokenEndIndex = 0;

            foreach (var tokenInfo in DecomposeIntoTokens(tokenSegments))
            {
                var (position, token, _, isOpenToken, _, _) = tokenInfo;
                builder.Append(text.Substring(lastTokenEndIndex, position - lastTokenEndIndex));
                
                if (rules.IsInterruptTag(tokenInfo.Tag) || rules.IsNewLineTag(tokenInfo.Tag))
                    CloseAllSingleTokens();
                
                if (tokenInfo.ShellStatus?.IsFrontShellNeed ?? false)
                    builder.Append(tokenInfo.ShellStatus.ShellTag.Start);
                
                var tag = tokenInfo.Tag;
                var translatedTag = translator.Translate(tag);
                builder.Append(isOpenToken ? translatedTag.Start : translatedTag.End);
                
                lastTokenEndIndex = position + token.Length;
                if (tag.End is null && isOpenToken && translatedTag.End is not null)
                    singleTagsCloseSymbols.Push(new TokenInfo(position, translatedTag.End) {ShellStatus = tokenInfo.ShellStatus});
            }

            builder.Append(text[lastTokenEndIndex..]);
            CloseAllSingleTokens();
            
            return builder.ToString();
        }
    }
}