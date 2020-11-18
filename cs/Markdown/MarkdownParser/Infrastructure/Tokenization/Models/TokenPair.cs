using System.Linq;
using System.Text;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Tokenization.Models
{
    public sealed class TokenPair : Token
    {
        public PairedToken Opening { get; }
        public Token[] Inner { get; }
        public PairedToken Closing { get; }

        public TokenPair(PairedToken opening, Token[] inner, PairedToken closing) 
            : base(opening.StartPosition, GetRawValue(opening, inner, closing))
        {
            Opening = opening;
            Inner = inner;
            Closing = closing;
        }

        private static string GetRawValue(PairedToken opening, Token[] inner, PairedToken closing)
        {
            var sb = new StringBuilder();
            sb.Append(opening.RawValue);
            sb.AppendJoin(string.Empty, inner.Select(x => x.RawValue));
            sb.Append(closing.RawValue);
            return sb.ToString();
        }
    }
}