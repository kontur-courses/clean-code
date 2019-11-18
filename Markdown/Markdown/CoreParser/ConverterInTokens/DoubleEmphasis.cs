using Markdown.ConverterInTokens;
using Markdown.Tokens;

namespace Markdown.ConverterTokens
{
    public class DoubleEmphasis : AbstractConverterInToken
    {
        public DoubleEmphasis() : base("__", "__")
        {
        }

        public override IToken GetCurrentToken(string Text, int startIndex, IToken[] nestedTokens)
        {
            return new DoubleEmphasisToken(Text, startIndex, nestedTokens);
        }
        
    }
}