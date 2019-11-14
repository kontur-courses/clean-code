using System;
using System.Collections.Generic;
using Markdown.IntermediateState;

namespace Markdown.Parsers
{
    class MarkdownParser : ILanguageParser
    {
        private string inputDocument;

        private Dictionary<string, Tag> languageTags;

        public MarkdownParser()
        {

        }

        public DocumentNode GetParsedDocument(string inputDocument)
        {
            this.inputDocument = inputDocument;
            // Find next open tag that can be used inter the other and check if this tag is closed
            throw new NotImplementedException();
        }

        private int FindNextTagIndex(int currentIndex)
        {
            throw  new NotImplementedException();
        }

        private int FindCloseTag(Tag tag, int startIndex, int endIndex)
        {
            throw new NotImplementedException();
        }
    }
}
