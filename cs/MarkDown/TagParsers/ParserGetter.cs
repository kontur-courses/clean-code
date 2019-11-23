using System.Collections.Generic;
using System.Linq;

namespace MarkDown.TagParsers
{
    public class ParserGetter
    {
        public readonly List<char> EscapeSymbols;
        public readonly HashSet<char> FirstTagChars;
        private readonly Dictionary<TagType, TagParser> tagParsers;

        public ParserGetter()
        {
            tagParsers = new Dictionary<TagType, TagParser>
            {
                {TagType.Strong, new StrongParser()},
                {TagType.Em, new EmParser()}
            };

            FirstTagChars = tagParsers.Values.Select(v => v.OpeningTags.md[0])
                .Concat(tagParsers.Values.Select(v => v.OpeningTags.html[0])).ToHashSet();

            EscapeSymbols = new List<char> {'\\'};
        }

        public List<TagParser> GetOrderedTagParsers()
        {
            return tagParsers.Values.ToList();
        }

        public TagParser GetParserFromType(TagType type)
        {
            return tagParsers[type];
        }
    }
}