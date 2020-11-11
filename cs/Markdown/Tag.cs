using System;
using System.Collections.Immutable;

namespace Markdown
{
    public static class Tag
    {
        public static readonly ImmutableDictionary<TagType, string> HtmlValue;

        public static bool IsTagInsideDigits(string text, TagToken token) => throw new NotImplementedException();

        public static bool IsTagInSameWord(string text, TagToken token) => throw new NotImplementedException();

        public static bool IsCorrectIntersection(string text, TagToken firstTag, TagToken secondTag) => throw new NotImplementedException();
    }
}