using System;
using System.Text;

namespace Markdown
{
    public class Italic : IHtmlTagCreator
    {
        private readonly string italicChar = "_";

        public StringBuilder GetHtmlTag(string markdownText, int openTagIndex)
        {
            var closingTagIndex = FindClosingTagIndex(markdownText);
            var htmlTag = CreateHtmlTag(markdownText, openTagIndex, closingTagIndex);

            throw new NotImplementedException();
        }

        private StringBuilder CreateHtmlTag(string markdownText, int openTagIndex, int closingTagIndex)
        {
            throw new NotImplementedException();
        }

        private int FindClosingTagIndex(string markdownText)
        {
            ProcessAnotherTag(markdownText);

            throw new NotImplementedException();
        }

        private StringBuilder ProcessAnotherTag(string markdownText)
        {
            var intersection = new Intersection(italicChar);

            throw new NotImplementedException();
        }
    }
}