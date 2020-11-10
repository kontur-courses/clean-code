namespace MarkdownParser.Infrastructure.Tokenization.Models
{
    public class TokenizationContext
    {
        public TokenizationContext(string source, int currentStartIndex, TokenPosition tokenPosition)
        {
            Source = source;
            CurrentStartIndex = currentStartIndex;
            TokenPosition = tokenPosition;
        }

        public string Source { get; }
        public int CurrentStartIndex { get; }
        
        public TokenPosition TokenPosition { get; }
    }
}