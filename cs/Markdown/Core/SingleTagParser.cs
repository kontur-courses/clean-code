using System.Collections.Generic;
using System.Linq;
using Markdown.Core.Rules;
using Markdown.Core.Tags;

namespace Markdown.Core
{
    class SingleTagParser
    {
        private readonly List<ISingleTag> tags;

        public SingleTagParser(IEnumerable<IRule> rules)
        {
            tags = rules.Where(rule => rule.SourceTag is ISingleTag)
                .Select(rule => rule.SourceTag as ISingleTag)
                .OrderByDescending(tag => tag.Opening.Length)
                .ToList();
        }

        public List<TagToken> ParseLine(string line)
        {
            return line == null
                ? new List<TagToken>()
                : tags.Where(tag => line.StartsWith(tag.Opening))
                    .Select(tag => new TagToken(0, tag, tag.Opening, true, false))
                    .ToList();
        }
    }
}