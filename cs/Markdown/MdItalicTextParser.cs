using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MdItalicTextParser : ParentParser
    {
        public override ParsingResult Parse(string mdText, int startBoundary, int endBoundary)
        {
            if (mdText[startBoundary] != Md.ItalicQuotes
                || mdText[startBoundary + 1] == ' ')
                return ParsingResult.Fail();
            var endQuotesIndex = FindEndQuotes(mdText, startBoundary, endBoundary);
            if (endQuotesIndex > endBoundary || endQuotesIndex == -1)
                return ParsingResult.Fail();
            var children = ParseChildren(TextType.ItalicText, mdText, startBoundary + 1, endQuotesIndex - 1);
            return children.Failure ? children : ParsingResult.Ok(children.Value, startBoundary, endQuotesIndex);
        }

        private static int FindEndQuotes(string mdText, int startBoundary, int endBoundary)
        {
            for (var i = startBoundary + 1; i <= endBoundary; i++)
            {
                if (mdText[i] == Md.ItalicQuotes && mdText[i - 1] != ' ')
                    return i;
            }
            return -1;
        }
    }
}