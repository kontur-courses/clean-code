namespace Markdown.Tokens
{
    public class DoubleEmphasisToken : AbstractToken
    {
        public DoubleEmphasisToken(string text, int indexTokenStart, IToken[] nestedTokens) : base(text, indexTokenStart, nestedTokens, text.Length + 4)
        {
        }
    }
}