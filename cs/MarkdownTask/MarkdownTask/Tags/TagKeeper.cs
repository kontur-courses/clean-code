using System;
using System.Collections.Generic;

namespace MarkdownTask.Tags
{
    public static class TagKeeper
    {
        private static readonly Dictionary<TagType, Tuple<string, string>> TagParts =
            new Dictionary<TagType, Tuple<string, string>>
            {
                { TagType.Italic, Tuple.Create(@"\<em>", @"\</em>") },
                { TagType.Bold, Tuple.Create(@"\<strong>", @"\</strong>") },
                { TagType.Header, Tuple.Create(@"\<h1>", @"\</h1>") }
            };

        public static Tag GetHtmlTagByType(TagType type)
        {
            return type switch
            {
                TagType.Italic => new Tag(TagType.Italic, TagParts[TagType.Italic].Item1, TagParts[TagType.Italic].Item2),
                TagType.Bold => new Tag(TagType.Bold, TagParts[TagType.Bold].Item1, TagParts[TagType.Bold].Item2),
                TagType.Header => new Tag(TagType.Header, TagParts[TagType.Header].Item1, TagParts[TagType.Header].Item2),
                _ => throw new ArgumentException("Uknown tag")
            };
        }
    }
}