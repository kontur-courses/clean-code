using Markdown.Tokens;

namespace Markdown.Writers.TagHtmlInfos;

public interface ITagHtmlInfo
{
    public Type TagType { get; }

    public string GetOpeningTag(Token token);
    
    public string GetClosingTag(Token token);
}