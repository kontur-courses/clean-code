using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Markdown
{
    public static class TagAnalyzer
    {
        private static readonly ImmutableDictionary<TagType, string> HtmlValue = new Dictionary<TagType, string>()
        {
            {TagType.Italic, "em"},
            {TagType.Bold, "strong"},
            {TagType.Header, "h1"},
            {TagType.Shield, ""},
            {TagType.Link, "a"},
            {TagType.NonTag, ""}
        }.ToImmutableDictionary();

        private static readonly ImmutableDictionary<TagType, int> SignLength = new Dictionary<TagType, int>
        {
            {TagType.Italic, 1},
            {TagType.Bold, 2},
            {TagType.Header, 2},
            {TagType.Shield, 1},
            {TagType.Link, 1},
            {TagType.NonTag, 0}
        }.ToImmutableDictionary();

        private static string GetWord(string line, int position)
        {
            var wordEndIndex = line.IndexOf(' ', position);
            var wordStartIndex = line.LastIndexOf(' ', position);
            if (wordEndIndex == -1)
                wordEndIndex = line.Length - 1;
            if (wordStartIndex == -1)
                wordStartIndex = 0;
            return line.Substring(wordStartIndex, wordEndIndex - wordStartIndex);
        }

        public static string GetDefaultHtmlValue(TagType t) => HtmlValue[t];

        public static int GetSignLength(TagType t) => SignLength[t];

        public static bool IsTagInsideWordWithDigits(string line, TagToken token) =>
            IsCoverPartOfWord(line, token) && GetWord(line, token.StartPosition).Any(char.IsDigit);

        public static bool IsTagInSameWord(string line, TagToken token) =>
            !line.Substring(token.StartPosition, token.ValueLength).Contains(" ");
        
        public static bool IsCoverPartOfWord(string line, TagToken token) =>
            token.StartPosition >= 1 && char.IsLetterOrDigit(line[token.StartPosition - 1])
            || token.EndPosition + token.TagSignLength < line.Length &&
            char.IsLetterOrDigit(line[token.EndPosition + token.TagSignLength]);
        
        public static bool IsCorrectIntersection(TagToken firstTag, TagToken secondTag) =>
            !firstTag.IsIntersectedWith(secondTag) || firstTag.Type == secondTag.Type;
        

        public static bool IsCorrectNesting(TagToken external, TagToken nested) => 
            !(nested.Type is TagType.Bold && nested.IsInsideOf(external) && external.Type is TagType.Italic)
        || nested.Type is TagType.Link;
    }
}