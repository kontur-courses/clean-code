using Markdown.Tokens;

namespace Markdown.CoreParser.ConverterInTokens
{
    public class DoubleEmphasis : AbstractConverterInToken
    {
        public DoubleEmphasis() : base("__", "__", new []{'_'})
        {
        }

        protected override IToken GetCurrentToken(string Text, int startIndex, IToken[] nestedTokens)
        {
            return new DoubleEmphasisToken(Text, startIndex, nestedTokens);
        }
        
    }
}