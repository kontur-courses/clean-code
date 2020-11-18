using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Tokenization.Models
{
    public sealed class PairedTokenData
    {
        public PairedTokenData(int index, PairedToken token)
        {
            Index = index;
            Token = token;
        }

        public int Index { get; }
        public PairedToken Token { get; }
    }
}