namespace MarkdownParser.Infrastructure.Tokenization.Abstract
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