using Markdown.Primitives;

namespace Markdown.Parsers;

public class TokenContext
{
    public bool HasWhiteSpace { get; private set; }
    public Token Token { get; }
    public bool OnMiddle { get; }
    public List<TagNode> Children { get; }


    public TokenContext(Token token, bool onMiddle = false)
    {
        Children = new List<TagNode>();
        OnMiddle = onMiddle;
        Token = token;
    }

    public void AddChild(TagNode node)
    {
        HasWhiteSpace |= node.Tag.Value.Contains(Characters.WhiteSpace);
        Children.Add(node);
    }

    public string ToText()
    {
        return Children.Count == 0
            ? Token.Value
            : Token.Value + string.Join("", Children.Select(x => x.ToText()));
    }

    public static bool IsHeader1(TokenContext context)
    {
        return context.Token.Type == TokenType.Header1;
    }

    public static bool IsLink(TokenContext context)
    {
        return context.Token.Type == TokenType.OpenSquareBracket;
    }
}