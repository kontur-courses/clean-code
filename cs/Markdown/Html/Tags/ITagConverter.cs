using Markdown.Markdown.Tokens;

namespace Markdown.Html.Tags;

public interface ITagConverter
{
    bool TryConvert(IToken token, out string resultContent);
}