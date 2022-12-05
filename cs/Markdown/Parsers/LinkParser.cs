using System.Text;
using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class LinkParser : ITokenParser
{
    private readonly InnerParser mainParser;

    public LinkParser(InnerParser mainParser)
    {
        this.mainParser = mainParser;
    }

    public TagNode Parse()
    {
        var token = mainParser.Current;
        return token.Type switch
        {
            TokenType.OpenSquareBracket => ParseOpenSquareBracket(),
            TokenType.CloseSquareBracket => ParseCloseSquareBracket(),
            _ => Tags.Text(token.Value).ToTagNode()
        };
    }

    private TagNode ParseCloseSquareBracket()
    {
        if (mainParser.AnyContext(TokenContext.IsLink) && mainParser.TryFlushContextsUntil(out var context, TokenContext.IsLink))
        {
            return ParseLinkAddressAfterClosedSquareBracket(context);
        }

        return Tokens.CloseSquareBracket.ToTextToken().ToTagNode();
    }

    private TagNode ParseLinkAddressAfterClosedSquareBracket(TokenContext linkContext)
    {
        if (mainParser.TryMoveNext(out var token))
        {
            if (token.Type == TokenType.OpenCircleBracket)
                return ParseLinkAddressAfterOpenCircleBracket(linkContext);

            mainParser.PushToBuffer(token);
        }

        linkContext.AddChild(Tokens.CloseSquareBracket.ToTagNode());
        return Tags.Text(linkContext.ToText()).ToTagNode();
    }

    private TagNode ParseLinkAddressAfterOpenCircleBracket(TokenContext linkContext)
    {
        var sb = new StringBuilder();
        while (mainParser.TryMoveNext(out var next))
        {
            switch (next.Type)
            {
                case TokenType.CloseCircleBracket:
                    return new TagNode(Tags.Link(sb.ToString()), linkContext.Children.ToArray());
                case TokenType.Escape:
                    sb.Append(ParseEscapeInLinkAddress());
                    break;
                default:
                    sb.Append(next.Value);
                    break;
            }
        }

        linkContext.AddChild(Tokens.CloseSquareBracket.ToTagNode());
        linkContext.AddChild(Tokens.OpenCircleBracket.ToTagNode());
        return Tags.Text($"{linkContext.ToText()}{sb}").ToTagNode();
    }

    private string ParseEscapeInLinkAddress()
    {
        if (mainParser.TryMoveNext(out var next) && next.Type == TokenType.CloseCircleBracket)
        {
            return Tokens.CloseCircleBracket.Value;   
        }

        return string.Join("", new[] { Tokens.Escape, Tokens.CloseCircleBracket }.Select(x => x.Value));
    }

    private TagNode ParseOpenSquareBracket()
    {
        var squareBracket = Tokens.OpenSquareBracket;
        if (mainParser.AnyContext(TokenContext.IsLink))
        {
            if (mainParser.TryFlushContextsUntil(out var node, TokenContext.IsLink))
            {
                mainParser.PushToBuffer(squareBracket);
                return mainParser.ToNode(node);
            }
        }

        if (mainParser.TryMoveNext(out var token))
        {
            mainParser.PushContext(new TokenContext(squareBracket));
            return mainParser.ParseToken(token);
        }

        return squareBracket.ToTagNode();
    }
}