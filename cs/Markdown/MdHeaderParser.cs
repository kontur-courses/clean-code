using System.Collections.Generic;

namespace Markdown
{
    public class MdHeaderParser : ParentParser
    {
        public MdHeaderParser()
        {
            childParsers = new IParser[] { new MdBoldTextParser(), new MdItalicTextParser(), new MdPlainTextParser() };
        }
        
        public override ParsingResult Parse(string mdText, int startBoundary, int endBoundary)
        {
            if (mdText[startBoundary] != Md.HeaderSymbol)
            {
                return ParsingResult.Fail();
            }
           
            var (paragraphEnd, childrenEnd) = MdParagraphParser.FindParagraphEnd(mdText, startBoundary, endBoundary);
            var children = ParseChildren(mdText, startBoundary + 1, childrenEnd);
            if (children.Failure)
                return children;
            children.Value.Type = "Header";
            children.StartIndex = startBoundary;
            children.EndIndex = paragraphEnd;
            return children;
        }
    }
}