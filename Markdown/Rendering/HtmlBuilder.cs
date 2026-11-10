using Markdown.Tokenization.Tokens;
using Markdown.Tokenization.Tokens.Abstractions;
using Markdown.Tokenization.Tokens.Implementations;
using System.Text;

namespace Markdown.Rendering;

public class HtmlBuilder
{
    private readonly StringBuilder _sb = new();
    private readonly Dictionary<Type, Action<Token>> _handlers = new();

    private static readonly Dictionary<Type, string> PairTags = new()
    {
        [typeof(EmToken)] = "em",
        [typeof(StrongToken)] = "strong"
    };

    private static readonly Dictionary<char, string> HtmlEntities = new()
    {
        ['&'] = "&amp;",
        ['<'] = "&lt;",
        ['>'] = "&gt;",
        ['"'] = "&quot;",
        ['\''] = "&#39;"
    };

    public HtmlBuilder()
    {
        Register<TextToken>(AppendTextToken);
        Register<EscapedToken>(AppendEscapedToken);
        Register<EmToken>(AppendPairToken);
        Register<StrongToken>(AppendPairToken);
        Register<LinkToken>(AppendLinkToken);
        Register<HeadingToken>(AppendHeadingToken);
    }

    public void Append(ResolvedTokens tokens)
    {
        var pendingBreaks = 0;

        foreach (var token in tokens)
        {
            if (token is LineBreakToken)
            {
                pendingBreaks++;
                continue;
            }

            if (token is HeadingToken { IsClosing: true })
            {
                AppendToken(token);
                AppendBreaks(Math.Max(0, pendingBreaks - 1));
            }
            else
            {
                AppendBreaks(pendingBreaks);
                AppendToken(token);
            }

            pendingBreaks = 0;
        }

        AppendBreaks(pendingBreaks);
    }

    public override string ToString()
        => _sb.ToString();

    private void AppendToken(Token token)
    {
        if (!_handlers.TryGetValue(token.GetType(), out var handler))
            throw new NotSupportedException($"Неизвестный токен: {token.GetType().Name}");

        handler(token);
    }

    private void AppendTextToken(TextToken token)
        => AppendEncoded(token.Source, token.Start, token.Length);

    private void AppendEscapedToken(EscapedToken token)
        => AppendEncoded(token.Source, token.Start + 1, 1);

    private void AppendPairToken(PairToken token)
    {
        var role = token.Context!.Role;

        if (role == PairRole.Text)
        {
            AppendEncoded(token.Source, token.Start, token.Length);
            return;
        }

        var tag = PairTags[token.GetType()];

        _sb.Append(role == PairRole.Open ? "<" : "</")
           .Append(tag)
           .Append('>');
    }

    private void AppendLinkToken(LinkToken token)
    {
        _sb.Append("<a href=\"");
        AppendEncoded(token.Source, token.UrlStart, token.UrlLength);
        _sb.Append("\">");
        AppendEncoded(token.Source, token.TextStart, token.TextLength);
        _sb.Append("</a>");
    }

    private void AppendHeadingToken(HeadingToken token)
        => _sb.Append(token.IsClosing ? "</h1>" : "<h1>");

    private void AppendBreaks(int count)
    {
        for (var i = 0; i < count; i++)
            _sb.Append("<br>");
    }

    private void AppendEncoded(string source, int start, int length)
    {
        for (var i = start; i < start + length; i++)
        {
            var symbol = source[i];

            if (HtmlEntities.TryGetValue(symbol, out var entity))
                _sb.Append(entity);
            else _sb.Append(symbol);
        }
    }

    private void Register<TToken>(Action<TToken> handler) where TToken : Token
    {
        _handlers[typeof(TToken)] = token => handler((TToken)token);
    }
}
