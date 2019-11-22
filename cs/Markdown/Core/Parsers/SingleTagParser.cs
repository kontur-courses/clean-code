using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Core.Rules;
using Markdown.Core.Tags;

namespace Markdown.Core.Parsers
{
    class SingleTagParser : IParser
    {
        private readonly List<ISingleTag> tags;

        public SingleTagParser(IEnumerable<ITag> tags)
        {
            this.tags = tags.OfType<ISingleTag>()
                .OrderByDescending(tag => tag.Opening.Length)
                .ToList();
        }

        private bool IsSkippedTag(string line, int index) => index != 0 && line[index - 1] == '\\';

        private int GetCountSpaceAtBeginningLine(string line)
        {
            var count = 0;
            foreach (var symbol in line)
            {
                if (char.IsWhiteSpace(symbol))
                    count++;
                else
                    return count;
            }

            return count;
        }

        public List<TagToken> ParseLine(string line)
        {
            var countSteps = Math.Min(5, GetCountSpaceAtBeginningLine(line) + tags.First().Opening.Length + 1);
            var positionsTags = tags
                .Select(tag => (tag, index: line.IndexOf(tag.Opening, StringComparison.Ordinal)))
                .OrderBy(tuple => tuple.index)
                .FirstOrDefault(tuple => tuple.index != -1 && tuple.index < countSteps);
            if (positionsTags.tag == null)
                return new List<TagToken>();
            var isSkipped = IsSkippedTag(line, positionsTags.index);
            var singleTagToken = new TagToken(positionsTags.index, positionsTags.tag, positionsTags.tag.Opening, true,
                isSkipped);
            return new List<TagToken> {singleTagToken};
        }
    }
}