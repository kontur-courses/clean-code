using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenMd
    {
        public string Token { get; set; }
        public string TokenWithoutMark { get; }

        public Mark Mark { get; set; }

        public TokenMd External { get; set; }
        public List<TokenMd> InnerTokens { get; set; }
        
        public string FormattedText { get; set; }

        public TokenMd(string token, Mark mark, TokenMd external = null)
        {
            InnerTokens = new List<TokenMd>();
            if (external != null)
                External = external;
            
            this.Mark = mark;
            Token = token;
            
            if (!(mark is EmptyMark))
                foreach (var symbol in mark.AllSymbols)
                {
                    TokenWithoutMark = token
                        .TrimStart(mark.DefiningSymbol.ToCharArray())
                        .TrimEnd(mark.AllSymbols.Last().ToCharArray());
                }
            else
                TokenWithoutMark = token;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(TokenMd))
                return false;

            var otherToken = obj as TokenMd;

            return Mark == otherToken.Mark
                    && Token == otherToken.Token
                    && InnerTokens.SequenceEqual(otherToken.InnerTokens);
        }

        public override int GetHashCode()
        {
            var hash = 0;

            if (Token != null)
                hash = Token.GetHashCode();

            if (InnerTokens == null) return hash;
            hash += InnerTokens.Sum(innerToken => innerToken.GetHashCode());

            return hash;
        }
    }
}