using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public interface IHtmlTagConverter
{
    public string ConvertTokensToHtmlText(
        Dictionary<TagType, IHtmlTagConverter> converters, 
        IReadOnlyList<Token> tokens,
        int start,
        out int readTokens);
}