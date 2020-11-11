using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Markdown
{
    public static class TagAnalyzer
    {
        private static readonly ImmutableDictionary<TagType, string> HtmlValue = new Dictionary<TagType, string>()
        {
            {TagType.Italic, "em"},
            {TagType.Bold, "strong"},
            {TagType.Header, "h1"},
        }.ToImmutableDictionary();

        private static readonly ImmutableDictionary<TagType, int> SignLength = new Dictionary<TagType, int>
        {
            {TagType.Italic, 1},
            {TagType.Bold, 2},
            {TagType.Header, 2}
        }.ToImmutableDictionary();

        public static string GetHtmlValue(TagType t) => throw new NotImplementedException();

        public static int GetSignLength(TagType t) => throw new NotImplementedException();

        public static bool IsTagInsideDigits(string text, TagToken token) => throw new NotImplementedException();

        public static bool IsTagInSameWord(string text, TagToken token) => throw new NotImplementedException();

        public static bool IsCorrectIntersection(string text, TagToken firstTag, TagToken secondTag) => throw new NotImplementedException();
    }
}