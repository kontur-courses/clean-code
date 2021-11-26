namespace Markdown
{
    
    public class ParsingResult : Result<HyperTextElement>
    {
        public readonly int StartIndex;
        public readonly int EndIndex;

        private ParsingResult(Status status, HyperTextElement value, int start, int end) : base(status, value)
        {
            StartIndex = start;
            EndIndex = end;
        }

        public static ParsingResult Success(HyperTextElement value, int startIndex, int endIndex)
        {
            return new ParsingResult(Status.Success, value, startIndex, endIndex);
        }

        public new static ParsingResult Fail(Status reason)
        {
            return new ParsingResult(reason, null, 0, 0);
        }
    }
}