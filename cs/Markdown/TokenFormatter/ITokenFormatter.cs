using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.TokenFormatter
{
    public interface ITokenFormatter
    {
        string Format(IEnumerable<Token> tokens);
    }
}