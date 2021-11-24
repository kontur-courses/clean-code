using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MdParagraphParser : ParentParser
    {
        public MdParagraphParser()
        {
            ChildParsers = new IParser[] { new MdBoldTextParser(), new MdItalicTextParser()};
        }

        public override ParsingResult Parse(string mdText, int startBoundary, int endBoundary)
        {
            var type = TextType.Paragraph;
            var childrenStart = startBoundary;
            if (mdText.ContainsAt(startBoundary, Md.HeaderSymbol + " "))
            {
                type = TextType.Header;
                childrenStart += 2;
            }
            var (paragraphEnd, childrenEnd) = FindParagraphEnd(mdText, startBoundary, endBoundary);
            var children = ParseChildren(type, mdText, childrenStart, childrenEnd);
            if (children.Failure)
                return children;
            children.StartIndex = startBoundary;
            children.EndIndex = paragraphEnd;
            return children;
        }

        private static (int paragraphEnd, int childBlockEnd)  FindParagraphEnd(string mdText, int startBoundary, int endBoundary)
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