using Markdown.Parser.Interface;
using Markdown.Token;

namespace Markdown.Parser.TokenHandler.Handlers;

public abstract class BaseTokenHandler : ITokenHandler
{
    protected readonly Delimiter Delimiter;

    public BaseTokenHandler(Delimiter delimiter)
    {
        Delimiter = delimiter;
    }

    public abstract bool TryHandle(ParsingContext context, out Token.Token token, out int skip);

    protected bool IsMatch(string text, int position, string pattern)
    {
        if (position + pattern.Length > text.Length)
            return false;

        return text.Substring(position, pattern.Length) == pattern;
    }
}