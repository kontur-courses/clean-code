namespace Markdown.Tokens
{
    public class SingleEmphasisToken : AbstractToken
    {
        public SingleEmphasisToken(string text, int indexTokenStart, IToken[] nestedTokens) : base(text, indexTokenStart, nestedTokens, text.Length + 2)
        {
        }
    }
}