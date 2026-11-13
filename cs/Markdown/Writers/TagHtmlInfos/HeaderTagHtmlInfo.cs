using Markdown.Tokens;

namespace Markdown.Writers.TagHtmlInfos;

public class HeaderTagHtmlInfo : ITagHtmlInfo
{
    public Type TagType => typeof(HeaderToken);

    public string GetOpeningTag(Token token)
        => "<h1>";

    public string GetClosingTag(Token token)
        => "</h1>";
}
