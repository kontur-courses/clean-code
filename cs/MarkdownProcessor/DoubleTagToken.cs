namespace MarkdownProcessor
{
    public class DoubleTagToken : IToken
    {
        public DoubleTagToken(string value, bool isOpening, int rank)
        {
            Value = value;
            IsOpening = isOpening;
            Rank = rank;
        }

        public string Value { get; }
        public int Rank { get; }
        public bool IsOpening { get; }
    }
}