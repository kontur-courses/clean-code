using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public abstract class BaseHtmlTagConverter(TagType tagType) : IHtmlTagConverter
{
    public readonly TagType HandledTag = tagType;

    public abstract string ConvertTokensToHtmlText(
        Dictionary<TagType, IHtmlTagConverter> converters,
        IReadOnlyList<Token> tokens,
        int start,
        out int readTokens);
    
    protected static string ConvertTokensToHtmlTextInTag(
        Dictionary<TagType, IHtmlTagConverter> converters,
        IReadOnlyList<Token> tokens,
        TagType tagType,
        ref int currentPosition)
    {
        var tagConverter = converters[tagType];
        var convertedString = 
            tagConverter.ConvertTokensToHtmlText(converters, tokens, currentPosition + 1, out var readTokens);
        currentPosition += readTokens;
        return convertedString;
    }
}