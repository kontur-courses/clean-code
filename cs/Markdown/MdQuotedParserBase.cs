namespace Markdown
{
    public class MdQuotedParserBase : ParserBase
    {
        private readonly TextType parsingType;
        private readonly string quotes;
        
        public MdQuotedParserBase(TextType parsingType, string quotes)
        {
            this.parsingType = parsingType;
            this.quotes = quotes;
        }
        
        public override ParsingResult Parse(StringWithShielding mdText, int startBoundary, int endBoundary)
        {
            var startStatus = CheckStartQuotes(mdText, startBoundary, endBoundary);
            if (startStatus != Status.Success)
                return ParsingResult.Fail(startStatus);
            var inWord = IsInsideWord(mdText, startBoundary, startBoundary + quotes.Length - 1);
            var endQuotesIndex = FindEndQuotes(mdText, startBoundary, endBoundary, inWord);
            if (endQuotesIndex.Status != Status.Success)
            {
                if (!inWord && endQuotesIndex.Status == Status.NotFound)
                    return ParsingResult.Success(new HyperTextElement<string>(TextType.PlainText, quotes),
                        startBoundary, startBoundary + quotes.Length - 1);
                return ParsingResult.Fail(endQuotesIndex.Status);
            }
            if (startBoundary + quotes.Length == endQuotesIndex.Value)
                return ParsingResult.Success(
                    new HyperTextElement<string>(TextType.PlainText, quotes + quotes),
                    startBoundary, startBoundary + quotes.Length * 2 - 1);
            var children = ParseChildren(parsingType, mdText, startBoundary + quotes.Length, endQuotesIndex.Value - 1);
            return children.Status != Status.Success ? children : ParsingResult.Success(children.Value, startBoundary, endQuotesIndex.Value + quotes.Length - 1);
        }

        private Status CheckStartQuotes(StringWithShielding mdText, int startBoundary, int endBoundary)
        {
            if (startBoundary + quotes.Length > endBoundary || !mdText.ContainsAt(startBoundary, quotes))
                return Status.NotFound;
            if (mdText[startBoundary + quotes.Length] == ' ')
                return Status.BadResult;
            if (IsInsideWordWithNumber(mdText, startBoundary, startBoundary + quotes.Length - 1))
                return Status.NotFound;
            return Status.Success;
        }

        private static bool IsInsideWord(StringWithShielding mdText, int leftIndex, int rightIndex)
        {
            if (leftIndex <= 0 || rightIndex >= mdText.Length - 1) 
                return false;
            return char.IsLetter(mdText[leftIndex - 1])  && char.IsLetter(mdText[rightIndex + 1]);
        }

        private static bool IsInsideWordWithNumber(StringWithShielding mdText, int leftIndex, int rightIndex)
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

        protected virtual Result<int> FindEndQuotes(StringWithShielding mdText, int startBoundary, int endBoundary, bool inSameWord)
        {
            var spaceWas = false;
            for (var i = startBoundary + quotes.Length; i <= endBoundary; i++)
            {
                if (mdText[i] == ' ')
                    spaceWas = true;
                if (inSameWord && spaceWas)
                    return Result<int>.Fail(Status.NotFound);
                if (!mdText.ContainsAt(i, quotes)) continue;
                if (mdText[i - 1] != ' ' && (!spaceWas || (i + quotes.Length - 1 == endBoundary || mdText[i + quotes.Length] == ' ')))
                    return Result<int>.Success(i);
                return Result<int>.Fail(Status.BadResult);
            }
            return Result<int>.Fail(Status.NotFound);
        }
    }
}