using Markdown.Tokens;

namespace Markdown.Writers.TagHtmlInfos;

public class LinkTagHtmlInfo : ITagHtmlInfo
{
    public Type TagType => typeof(LinkToken);

    public string GetOpeningTag(Token token)
        => string.Format($"<a href=\"{((LinkToken)token).Url}\">");

    public string GetClosingTag(Token token)
        => "</a>";

}