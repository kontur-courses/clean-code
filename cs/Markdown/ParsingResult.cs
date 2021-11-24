namespace Markdown
{
    public class ParsingResult
    {
        public bool Success;
        public bool Failure => !Success;
        public HyperTextElement Value;
        public int StartIndex;
        public int EndIndex;

        private ParsingResult(bool success, HyperTextElement value, int start, int end)
        {
            Success = success;
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