using Markdown.Dto;
using Markdown.TagUtils.Abstractions;
using Markdown.TagUtils.Implementations;

public class TagStrategyFactory(ITagContext context)
{
    private readonly Dictionary<TokenType, BaseTagStrategy> strategies = new()
    {
        [TokenType.Italic] = new ItalicStrategy(context),
        [TokenType.Strong] = new StrongStrategy(context),
        [TokenType.Link] = new LinkStrategy(context),
        [TokenType.Header] = new HeaderStrategy(context),
        [TokenType.End] = new EndStrategy(context),
        [TokenType.Text] = new TextStrategy(context),
        [TokenType.Escape] = new EscapeStrategy(context)
    };

    public BaseTagStrategy? Get(TokenType type)
        => strategies.GetValueOrDefault(type);
}