using System.Collections.Generic;

namespace Markdown;

public class Token
{
    public TokenType Type;
    public required string Value { get; init; }
}