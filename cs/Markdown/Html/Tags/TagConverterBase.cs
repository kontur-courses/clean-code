using Markdown.Markdown.Tokens;

namespace Markdown.Html.Tags;

public class TagConverterBase(TagType tagType) : ITagConverter
{
    private TagType HtmlTagType { get; } = tagType;

    private string GetOpenTag =>
        $"<{HtmlTagCreator.GetStringContent(HtmlTagType)}>";

    private string GetCloseTag =>
        $"</{HtmlTagCreator.GetStringContent(HtmlTagType)}>";

    public bool TryConvert(IToken token, out string resultContent)
    {
        resultContent = token.IsCloseTag ? GetCloseTag : GetOpenTag;
        return IsCorrectTag(token.TagType);
    }

    private bool IsCorrectTag(TagType tokenTagType) =>
        tokenTagType == HtmlTagType;
}