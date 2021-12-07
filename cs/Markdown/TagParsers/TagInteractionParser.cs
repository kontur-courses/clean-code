using System.Collections.Generic;
using System.Data;
using Markdown.TagEvents;
using NUnit.Framework;

namespace Markdown.TagParsers
{
    public class TagInteractionParser : ITagParser
    {
        private readonly List<TagEvent> tagEvents;
        private readonly Stack<TagEvent> tagPairs;

        public TagInteractionParser(List<TagEvent> tagEvents)
        {
            this.tagEvents = tagEvents;
            tagPairs = new Stack<TagEvent>();
        }
        public List<TagEvent> Parse()
        {
            return tagEvents;
        }

    }
}
