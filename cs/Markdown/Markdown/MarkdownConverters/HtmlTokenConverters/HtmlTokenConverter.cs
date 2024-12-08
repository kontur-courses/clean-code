namespace Markdown;

public abstract class HtmlTokenConverter(TokenType typeOfToken, string openTag, string closeTag)
{
    public readonly TokenType TypeOfToken = typeOfToken;
    public readonly string OpenTag = openTag;
    public readonly string CloseTag = closeTag;

    public virtual string Convert(Token token)
    {
        if (token.Type != TypeOfToken)
            throw new ArgumentException($"{nameof(TokenType)} should be {nameof(TypeOfToken)} but was {token.Type}");

        return $"{OpenTag}{token.Content}{CloseTag}";
    }
}
