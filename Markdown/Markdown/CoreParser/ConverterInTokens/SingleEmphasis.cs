using Markdown.Tokens;

namespace Markdown.CoreParser.ConverterInTokens
{
    public class SingleEmphasis : AbstractConverterInToken
    {
        public SingleEmphasis() : base("_", "_", new []{'_'})
        {
        }

        protected override IToken GetCurrentToken(string Text, int startIndex, IToken[] nestedTokens)
        {
            return new SingleEmphasisToken(Text, startIndex, nestedTokens);
        }
    }

 }