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
                || startBoundary >= endBoundary - 1 )
                return ParsingResult.Fail(Status.NotFound);
            var inWord = IsInsideWord(mdText, startBoundary, startBoundary + 1);
            if (mdText[startBoundary + 2] == ' ')
                return ParsingResult.Fail(Status.BadResult);
            if (IsInsideWordWithNumber(mdText, startBoundary, startBoundary + 1))
                return ParsingResult.Fail(Status.NotFound);
            var endQuotesIndex = FindEndQuotes(mdText, startBoundary, endBoundary, inWord);
            if (endQuotesIndex > endBoundary || endQuotesIndex == -1)
                return inWord ? ParsingResult.Fail(Status.NotFound) : ParsingResult.Fail(Status.BadResult);
            var children = ParseChildren(TextType.BoldText, mdText, startBoundary + 2, endQuotesIndex - 1);
            return children.Status != Status.Success ? children : ParsingResult.Success(children.Value, startBoundary, endQuotesIndex + 1);
        }
        
        public static bool IsInsideWord(StringWithShielding mdText, int leftIndex, int rightIndex)
        {
            if (leftIndex <= 0 || rightIndex >= mdText.Length - 1) 
                return false;
            return char.IsLetter(mdText[leftIndex - 1])  && char.IsLetter(mdText[rightIndex + 1]);
        }

        public static bool IsInsideWordWithNumber(StringWithShielding mdText, int leftIndex, int rightIndex)
        {
            if (leftIndex <= 0 || rightIndex >= mdText.Length - 1) 
                return false;
            if (mdText[leftIndex - 1] == ' ' || mdText[rightIndex + 1] == ' ')
                return false;
            for (var i = leftIndex - 1; i >= 0 && mdText[i] != ' '; i--)
                if (char.IsDigit(mdText[i]))
                    return true;
            for (var i = rightIndex + 1; i < mdText.Length && mdText[i] != ' '; i++)
                if (char.IsDigit(mdText[i]))
                    return true;
            return false;
        }

        private static int FindEndQuotes(StringWithShielding mdText, int startBoundary, int endBoundary, bool inSameWord)
        {
            for (var i = startBoundary + 2; i <= endBoundary; i++)
            {
                if (inSameWord && mdText[i] == ' ')
                    return -1;
                if (mdText.ContainsAt(i, Md.BoldQuotes) && (mdText[i - 1] != ' ' || inSameWord))
                    return i;
            }
            return -1;
        }
    }
}