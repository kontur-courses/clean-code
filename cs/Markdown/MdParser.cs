using System.Collections.Generic;
using Markdown.Tag_Classes;

namespace Markdown
{
    public class MdParser
    {
        private static HashSet<char> _tagSymbols =
            new HashSet<char> {'#', '_', '\n'};

        private readonly Stack<TagEvent> _openedTags;
        private readonly List<TagEvent> _parsedTags;

        public MdParser()
        {
            _openedTags = new Stack<TagEvent>();
            _parsedTags = new List<TagEvent>();
        }

        public IReadOnlyList<TagEvent> Parse(string rawInput)
        {
            return _parsedTags;
        }
    }
}
