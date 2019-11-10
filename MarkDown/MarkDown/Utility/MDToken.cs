namespace MarkDown
{
    public class MDToken
    {
        public readonly string Value;
        public readonly int StartIndex;
        public readonly int Length;

        public MDToken(string value, int startIndex, int length)
        {
            Value = value;
            StartIndex = startIndex;
            Length = length;
        }

        public int GetIndexNextToToken()
        {
            return StartIndex + Length;
        }
    }
}