using System.Collections.Generic;

namespace Markdown.Token
{
    public interface ITokenConverter
    {
        IEnumerable<IToken> Convert(string source);
        string Convert(IEnumerable<IToken> source);
    }
}