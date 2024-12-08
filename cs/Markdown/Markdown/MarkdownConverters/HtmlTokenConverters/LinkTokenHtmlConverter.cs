namespace Markdown;

public class LinkTokenHtmlConverter : HtmlTokenConverter
{
    public LinkTokenHtmlConverter() : base(TokenType.Link, "<a", "</a>")
    {
    }

    public override string Convert(Token token)
    {
        if (token.Type != TypeOfToken)
            throw new ArgumentException($"{nameof(TokenType)} should be {nameof(TypeOfToken)} but was {token.Type}");

        if (token is TokenWithArgument linkToken)
            return $"{OpenTag} href=\"{linkToken.Argument}\">{linkToken.Content}{CloseTag}";
        throw new ArgumentException($"{nameof(Token)} should be {nameof(TokenWithArgument)}");
    }
}

