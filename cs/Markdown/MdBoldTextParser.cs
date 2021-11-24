using System.Collections.Generic;

namespace Markdown
{
    public class MdBoldTextParser : ParentParser
    {
        public MdBoldTextParser()
        {
            ChildParsers = new IParser[] { new MdItalicTextParser() };
        }
        
        public override ParsingResult Parse(string mdText, int startBoundary, int endBoundary)
        {
            if (!mdText.ContainsAt(startBoundary, Md.BoldQuotes)
                || mdText[startBoundary + 2] == ' ')
                return ParsingResult.Fail();
            var endQuotesIndex = FindEndQuotes(mdText, startBoundary, endBoundary);
            if (endQuotesIndex > endBoundary || endQuotesIndex == -1)
                return ParsingResult.Fail();
            var children = ParseChildren(TextType.BoldText, mdText, startBoundary + 2, endQuotesIndex - 1);
            return children.Failure ? children : ParsingResult.Ok(children.Value, startBoundary, endQuotesIndex + 1);
        }

        private static int FindEndQuotes(string mdText, int startBoundary, int endBoundary)
        {
            for (var i = startBoundary + 2; i <= endBoundary; i++)
            {
                if (mdText.ContainsAt(i, Md.BoldQuotes) && mdText[i - 1] != ' ')
                    return i;
            }
            return -1;
        }
    }
}