using Markdown.Tokens;

namespace Markdown.Writers.TagHtmlInfos;

public class DoubleUnderscoreTagHtmlInfo : ITagHtmlInfo
{
    public Type TagType => typeof(DoubleUnderscoreToken);
    
    public string GetOpeningTag(Token token)
        => "<strong>";

    public string GetClosingTag(Token token)
        => "</strong>";
}
