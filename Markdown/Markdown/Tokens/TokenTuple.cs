using System.Collections.Generic;

namespace Markdown
{
    public class TokenTuple
    {
        public IEnumerable<Token> SeparetedPart { get;}
        public IEnumerable<Token> OtherTokens { get;}

        public TokenTuple() { }

        public TokenTuple(IEnumerable<Token> separetedPart, IEnumerable<Token> otherTokens)
        {
            SeparetedPart = separetedPart;
            OtherTokens = otherTokens;
        }
    }
}
