using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public abstract class UnderlineParser
{
    private TokenCollectionParser mainParser;

    protected UnderlineParser(TokenCollectionParser mainParser)
    {
        this.mainParser = mainParser;
    }

    protected TagNode ParseUnderline(Token token)
    {
        if (mainParser.TryPopContext(out var context))
        {
            if (CanCloseContext(context, token))
            {
                if (context.Children.All(n => n.Tag.Type == TagType.Text))
                {
                    var textParts = context.Children.Select(n => n.Tag.Value);

                    var text = string.Join("", textParts);

                    return IsNeedParseAsText(context, text)
                        ? Tokens.Text(string.Join("", new[] { token, Tokens.Text(text), token }.Select(x => x.Value)))
                            .ToTagNode()
                        : new TagNode(token.ToTag(), Tokens.Text(text).ToTagNode());
                }

                return TryParseNonText(context, out var node)
                    ? node
                    : new TagNode(token.ToTag(), context.Children.ToArray());
            }

            mainParser.PushContext(context);
        }


        var onMiddle = IsNextNonWhiteSpace() && IsPreviousNonWhiteSpace();
        if (IsNextNonWhiteSpace() && mainParser.TryMoveNext(out var next))
        {
            mainParser.PushContext(new TokenContext(token, onMiddle));
            return mainParser.ParseToken(next);
        }

        return token.ToTextToken().ToTagNode();
    }

    private bool CanCloseContext(TokenContext tokenContext, Token token)
    {
        var onMiddle = tokenContext.OnMiddle || IsNextNonWhiteSpace() && IsPreviousNonWhiteSpace();

        return tokenContext.Token.Type == token.Type
               && IsPreviousNonWhiteSpace()
               && (onMiddle && !tokenContext.HasWhiteSpace || !onMiddle);
    }

    private bool IsNextNonWhiteSpace()
    {
        return mainParser.TryGetNextToken(out var token)
               && token.Type == TokenType.Text
               && token.Value.Length > 0
               && char.IsLetterOrDigit(token.Value[0]);
    }

    private bool IsPreviousNonWhiteSpace()
    {
        return mainParser.TryGetPreviousToken(out var token)
               && token.Type == TokenType.Text
               && token.Value.Length > 0
               && char.IsLetterOrDigit(token.Value[^1]);
    }

    protected abstract bool TryParseNonText(TokenContext context, out TagNode tag);

    private static bool IsNeedParseAsText(TokenContext context, string text)
    {
        return text.All(char.IsDigit) || context.OnMiddle && text.Any(x => !char.IsLetter(x));
    }
}