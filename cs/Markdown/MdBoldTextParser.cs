using System.Collections.Generic;

namespace Markdown
{
    public class MdBoldTextParser : ParentParser
    {
        public MdBoldTextParser()
        {
            childParsers = new IParser[] { new MdItalicTextParser(), new MdPlainTextParser() };
        }
        
        public override ParsingResult Parse(string mdText, int startBoundary, int endBoundary)
        {
            if (!mdText.ContainsAt(startBoundary, Md.BoldQuotes)
                || mdText[startBoundary + 2] == ' ')
                return ParsingResult.Fail();
            var end = FindEndQuotes(mdText, startBoundary, endBoundary);
            if (end > endBoundary || end == -1)
                return ParsingResult.Fail();
            var value = mdText.Substring(startBoundary + 2,   end - startBoundary - 2);
            var element = new HyperTextElement("BoldText", value);
            return ParsingResult.Ok(element, startBoundary, end + 1);
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