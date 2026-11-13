using Markdown.Tokens;

namespace Markdown.Writers.TagHtmlInfos;

public class UnderscoreTagHtmlInfo : ITagHtmlInfo
{
    public Type TagType =>  typeof(UnderscoreToken);

    public string GetOpeningTag(Token token)
        => "<em>";

    public string GetClosingTag(Token token)
        => "</em>";
}