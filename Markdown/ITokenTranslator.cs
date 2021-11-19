using System;

namespace Markdown
{
    public interface ITokenTranslator
    {
        Token Translate(Token token);
    }
}