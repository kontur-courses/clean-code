using Markdown.Markdown.Attributes;
using Markdown.Markdown.Tokens;

namespace Markdown.Html.Tags;

public class ImageTagConverter : ITagConverter
{
    public bool TryConvert(IToken token, out string resultContent)
    {
        resultContent = IsHasAttributes(token)
            ? GetResultContent(token)
            : string.Empty;
        return !string.IsNullOrEmpty(resultContent);
    }

    private static string GetResultContent(IToken token) =>
        $"<{HtmlTagCreator.GetStringContent(token.TagType)} " +
        $"src=\"{token.Attributes![AttributeType.Src].Value}\" " +
        $"alt=\"{token.Attributes[AttributeType.Alt].Value}\">";

    private static bool IsHasAttributes(IToken token) =>
        token.Attributes != null &&
        token.Attributes.ContainsKey(AttributeType.Alt) &&
        token.Attributes.ContainsKey(AttributeType.Src);
}