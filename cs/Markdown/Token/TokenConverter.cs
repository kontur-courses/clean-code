using System.Collections.Generic;

namespace Markdown.Token
{
    public class TokenConverter : ITokenConverter
    {
        public IEnumerable<IToken> Convert(string source)
        {
            throw new System.NotImplementedException();
        }

        public string Convert(IEnumerable<IToken> source)
        {
            throw new System.NotImplementedException();
        }
    }
}