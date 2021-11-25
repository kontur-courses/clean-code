namespace Markdown
{
    public class MdItalicTextParser : ParserBase
    {
        public static readonly MdItalicTextParser Default = new MdItalicTextParser();
        
        private MdItalicTextParser(){}
        
        public override ParsingResult Parse(StringWithShielding mdText, int startBoundary, int endBoundary)
        {
            if (mdText[startBoundary] != Md.ItalicQuotes
                || startBoundary == endBoundary )
                return ParsingResult.Fail(Status.NotFound);
            var inWord = MdBoldTextParser.IsInsideWord(mdText, startBoundary, startBoundary + 1);
            if ( mdText[startBoundary + 1] == ' ')
                return ParsingResult.Fail(Status.BadResult);
            if (MdBoldTextParser.IsInsideWordWithNumber(mdText, startBoundary, startBoundary))
                return ParsingResult.Fail(Status.NotFound);
            var endQuotesIndex = FindEndQuotes(mdText, startBoundary, endBoundary, inWord);
            if (endQuotesIndex > endBoundary || endQuotesIndex == -1)
                return  inWord ? ParsingResult.Fail(Status.NotFound) : ParsingResult.Fail(Status.BadResult);
            var children = ParseChildren(TextType.ItalicText, mdText, startBoundary + 1, endQuotesIndex - 1);
            return children.Status != Status.Success ? children : ParsingResult.Success(children.Value, startBoundary, endQuotesIndex);
        }

        

        private static int FindEndQuotes(StringWithShielding mdText, int startBoundary, int endBoundary, bool inSameWord)
        {
            for (var i = startBoundary + 1; i <= endBoundary; i++)
            {
                if (inSameWord && mdText[i] == ' ')
                    return -1;
                if (mdText[i] == Md.ItalicQuotes)
                {
                    if (i < mdText.Length - 1 && mdText[i + 1] == Md.ItalicQuotes)
                    {
                        i++;
                        continue;
                    }
                    return mdText[i - 1] != ' ' || inSameWord ? i : -1;
                }
            }
            return -1;
        }
    }
}