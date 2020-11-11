namespace MarkdownParser.Infrastructure.Tokenization.Models
{
    public class TokenizationContext
    {
        public TokenizationContext(string source, int currentStartIndex)
        {
            Source = source;
            CurrentStartIndex = currentStartIndex;
        }

        public string Source { get; }
        public int CurrentStartIndex { get; }
    }
}