using System.Collections.Generic;

namespace Markdown
{
    public class MdBoldTextParser : ParserBase
    {
        public static readonly MdBoldTextParser Default = new MdBoldTextParser();
        private MdBoldTextParser()
        {
            ChildParsers = new IParser[] { MdItalicTextParser.Default };
        }
        
        public override ParsingResult Parse(StringWithShielding mdText, int startBoundary, int endBoundary)
        {
            if (!mdText.ContainsAt(startBoundary, Md.BoldQuotes)
                || mdText[startBoundary + 2] == ' ')
                return ParsingResult.Fail();
            var endQuotesIndex = FindEndQuotes(mdText, startBoundary, endBoundary);
            if (endQuotesIndex > endBoundary || endQuotesIndex == -1)
                return ParsingResult.Fail();
            var children = ParseChildren(TextType.BoldText, mdText, startBoundary + 2, endQuotesIndex - 1);
            return !children.IsSuccess ? children : ParsingResult.Ok(children.Value, startBoundary, endQuotesIndex + 1);
        }

        private static int FindEndQuotes(StringWithShielding mdText, int startBoundary, int endBoundary)
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