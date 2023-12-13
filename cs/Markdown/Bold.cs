using System;
using System.Text;

namespace Markdown
{
    public class Bold : IHtmlTagCreator
    {
        private readonly string boldChar = "__";

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
            Intersection intersection = new Intersection(boldChar);

            throw new NotImplementedException();
        }
    }
}