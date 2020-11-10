using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownParser
    {
        private string textForParsing;

        public MarkdownParser(string textForParsing)
        {
            this.textForParsing = textForParsing;
        }

        public List<Tag> Tags { get; }
        public string TextWithoutEscapeCharacters { get; private set; }

        public void Parse()
        {
            throw new NotImplementedException();
        }

        private bool TryReadTag(int position, out Tag tag)
        {
            throw new NotImplementedException();
        }

        private bool CanTagBeOpen(int position, TagType tagType)
        {
            throw new NotImplementedException();
        }

        private bool CanTagBeClose(int position, TagType tagType)
        {
            throw new NotImplementedException();
        }

        private void GroupTags()
        {
            throw new NotImplementedException();
        }
    }
}