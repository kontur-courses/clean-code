namespace Markdown
{
    public enum Status
    {
        Success,
        NotFound,
        BadResult
    }
    
    public class ParsingResult
    {
        public readonly Status Status;
        public readonly HyperTextElement Value;
        public readonly int StartIndex;
        public readonly int EndIndex;

        private ParsingResult(Status status, HyperTextElement value, int start, int end)
        {
            Status = status;
            Value = value;
            StartIndex = start;
            EndIndex = end;
        }

        public static ParsingResult Success(HyperTextElement value, int startIndex, int endIndex)
        {
            return new ParsingResult(Status.Success, value, startIndex, endIndex);
        }

        public static ParsingResult Fail(Status reason)
        {
            return new ParsingResult(reason, null, 0, 0);
        }
    }
}