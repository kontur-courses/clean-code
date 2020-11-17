using System.Linq;
using System.Text;

namespace MarkdownParser.Infrastructure.Tokenization.Abstract
{
    public class TokenPair : Token
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
            sb.Append(opening);
            sb.AppendJoin(string.Empty, inner.Select(x => x.RawValue));
            sb.Append(closing);
            return sb.ToString();
        }
    }
}