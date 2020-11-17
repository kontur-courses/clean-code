using System.Collections.Generic;

namespace Markdown.TokenConverters
{
    public interface ITokenConverter
    {
        TokenType TokenType { get; }
        string ToString(TextToken token, Dictionary<TokenType,ITokenConverter> tokenConverters);
    }
}