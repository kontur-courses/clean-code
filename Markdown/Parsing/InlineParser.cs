using Markdown.Models;

namespace Markdown.Parsing;

// обработка курсива, жирного текста и экранирования
internal sealed class InlineParser
{
    public string Parse(string text)
    {
        throw new NotImplementedException();
    }

    private IReadOnlyList<InlineToken> Tokenize(string text)
    {
        throw new NotImplementedException();
    }

    private IReadOnlyList<DelimiterPair> FindValidDelimiterPairs(
        string text,
        IReadOnlyList<InlineToken> tokens)
    {
        throw new NotImplementedException();
    }

    private bool CanOpen(string text, InlineToken token)
    {
        throw new NotImplementedException();
    }

    private bool CanClose(string text, InlineToken token)
    {
        throw new NotImplementedException();
    }

    private string RenderTokens(
        IReadOnlyList<InlineToken> tokens,
        IReadOnlyList<DelimiterPair> pairs)
    {
        throw new NotImplementedException();
    }
}