using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MdItalicTextParser : IParser
    {
        public ParsingResult Parse(string mdText, int startBoundary, int endBoundary)
        {
            if (mdText[startBoundary] != Md.ItalicQuotes
                || mdText[startBoundary + 1] == ' ')
                return ParsingResult.Fail();
            var end = FindEndQuotes(mdText, startBoundary, endBoundary);
            if (end > endBoundary || end == -1)
                return ParsingResult.Fail();
            var value = mdText.Substring(startBoundary + 1,   end - startBoundary - 1);
            var element = new HyperTextElement("ItalicText", value);
            return ParsingResult.Ok(element, startBoundary, end);
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