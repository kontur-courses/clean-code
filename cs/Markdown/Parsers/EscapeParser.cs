using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class EscapeParser : ITokenParser
{
    private readonly TokenCollectionParser mainParser;

    public EscapeParser(TokenCollectionParser mainParser)
    {
        this.mainParser = mainParser;
    }

    public TagNode Parse()
    {
        if (mainParser.TryMoveNext(out var next))
        {
            return next.Type switch
            {
                TokenType.Italic => next.ToTextToken().ToTagNode(),
                TokenType.Escape => next.ToTextToken().ToTagNode(),
                TokenType.Bold => EscapeBold(),
                TokenType.OpenSquareBracket => Tokens.OpenSquareBracket.ToTextToken().ToTagNode(),
                TokenType.CloseSquareBracket => EscapeSquareBracket(next),
                _ => Tokens.Text(string.Join("", new[] { Tokens.Escape, next }.Select(x => x.Value))).ToTagNode()
            };
        }

        return Tokens.Escape.ToTextToken().ToTagNode();
    }

    private TagNode EscapeSquareBracket(Token token)
    {
        return mainParser.AnyContext(TokenContext.IsLink)
            ? token.ToTextToken().ToTagNode()
            : Tags.Text(string.Join("", new[] { Tokens.Escape, token }.Select(x => x.Value))).ToTagNode();
    }

    private TagNode EscapeBold()
    {
        var italic = Tokens.Italic;
        mainParser.PushToBuffer(italic);
        return italic.ToTextToken().ToTagNode();
    }
}