namespace Markdown
{
    public class ParsingResult
    {
        public readonly bool IsSuccess;
        public readonly HyperTextElement Value;
        public readonly int StartIndex;
        public readonly int EndIndex;

        private ParsingResult(bool isSuccess, HyperTextElement value, int start, int end)
        {
            IsSuccess = isSuccess;
            Value = value;
            StartIndex = start;
            EndIndex = end;
        }

        public static ParsingResult Ok(HyperTextElement value, int startIndex, int endIndex)
        {
            return new ParsingResult(true, value, startIndex, endIndex);
        }

        public static ParsingResult Fail()
        {
            return new ParsingResult(false, null, 0, 0);
        }
    }
}