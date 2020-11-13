using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Analyzer
    {
        private readonly HashSet<TextSelectionType> _FoundSelectionTypes;
        private readonly Stack<Tag> _startTags;

        public Analyzer()
        {
            _FoundSelectionTypes = new HashSet<TextSelectionType>();
            _startTags = new Stack<Tag>();
        }

        public List<IOrderedEnumerable<Tag>> GetTagsForAllParagraphs(string[] paragraphs)
        {
            throw new NotImplementedException();
        }
    }
}