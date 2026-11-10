using System.Collections.Generic;

namespace Markdown;

public interface ILexer
{
    List<Token> GetTokens(string text);
}