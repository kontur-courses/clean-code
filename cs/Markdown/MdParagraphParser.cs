using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MdParagraphParser : ParentParser
    {
        public MdParagraphParser()
        {
            childParsers = new IParser[] { new MdBoldTextParser(), new MdItalicTextParser(), new MdPlainTextParser() };
        }
        
        public override ParsingResult Parse(string mdText, int startBoundary, int endBoundary)
        {
            var (paragraphEnd, childrenEnd) = FindParagraphEnd(mdText, startBoundary, endBoundary);
            var children = ParseChildren(mdText, startBoundary, childrenEnd);
            if (children.Failure)
                return children;
            children.Value.Type = "Paragraph";
            children.StartIndex = startBoundary;
            children.EndIndex = paragraphEnd;
            return children;
        }

        public static (int paragraphEnd, int childBlockEnd)  FindParagraphEnd(string mdText, int startBoundary, int endBoundary)
        {
            var paragraphEnd = mdText.IndexOf('\n', startBoundary);
            if (paragraphEnd < 0 || paragraphEnd > endBoundary)
                paragraphEnd = endBoundary + 1;
            var childrenEnd = paragraphEnd - 1;
            if (mdText[childrenEnd] == '\r')
                childrenEnd--;
            return (paragraphEnd, childrenEnd);
        }
    }
}