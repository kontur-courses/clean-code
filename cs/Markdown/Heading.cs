using System;
using System.Text;

namespace Markdown
{
    public class Heading : IHtmlTagCreator
    {
        private readonly string headingChar = "#";

        public StringBuilder GetHtmlTag(string markdownText, int openTagIndex)
        {
            ProcessAnotherTag(markdownText);
            var htmlTag = CreateHtmlTag(markdownText, openTagIndex);

            throw new NotImplementedException();
        }

        private StringBuilder CreateHtmlTag(string markdownText, int openTagIndex)
        {
            throw new NotImplementedException();
        }

        private StringBuilder ProcessAnotherTag(string markdownText)
        {
            var intersection = new Intersection(headingChar);
            intersection.HaveIntersection(markdownText, headingChar, 0);

            throw new NotImplementedException();
        }
    }
}