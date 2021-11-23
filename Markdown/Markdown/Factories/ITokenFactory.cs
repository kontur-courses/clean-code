using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Factories
{
    public interface ITokenFactory<T>
        where T : IToken
    {
        public T NewToken(TokenType type, string value);
    }
}