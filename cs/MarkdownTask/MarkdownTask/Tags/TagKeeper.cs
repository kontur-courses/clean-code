using System;
using System.Collections.Generic;

namespace MarkdownTask.Tags
{
    public static class TagKeeper
    {
        private static readonly Dictionary<TagType, Tuple<string, string>> TagParts =
            new Dictionary<TagType, Tuple<string, string>>
            {
                { TagType.SingleHighlight, Tuple.Create(@"\<em>", @"\</em>") },
                { TagType.DoubleHighlight, Tuple.Create(@"\<strong>", @"\</strong>") },
                { TagType.Header, Tuple.Create(@"\<h1>", @"\</h1>") }
            };

        public static Tag GetHtmlTagByType(TagType type)
        {
            switch (type)
            {
                case TagType.SingleHighlight:
                    return new Tag(TagType.SingleHighlight,
                        TagParts[TagType.SingleHighlight].Item1,
                        TagParts[TagType.SingleHighlight].Item2);

                case TagType.DoubleHighlight:
                    return new Tag(TagType.DoubleHighlight,
                        TagParts[TagType.DoubleHighlight].Item1,
                        TagParts[TagType.DoubleHighlight].Item2);

                case TagType.Header:
                    return new Tag(TagType.Header,
                        TagParts[TagType.Header].Item1,
                        TagParts[TagType.Header].Item2);

                default:
                    throw new ArgumentException("Unknown tag");
            }
        }
    }
}