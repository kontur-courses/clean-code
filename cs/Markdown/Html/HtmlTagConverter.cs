using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Html;

public class HtmlTagConverter
{
    private readonly ITagConverter[] htmlTags =
    [
        new HeaderTagConverter(),
        new StrongTagConverter(),
        new ItalicTagConverter(),
        new ImageTagConverter()
    ];

    public string Convert(IToken token)
    {
        if (token.TagType is TagType.None)
            return token.Content;
        
        foreach (var htmlTag in htmlTags)
        {
            var isConvertedToHtml = htmlTag.TryConvert(token, out var resultHtmlTag);

            if (isConvertedToHtml)
                return resultHtmlTag;
        }

        throw new ArgumentException($"{nameof(token.Content)} doesn't match to any html tag.");
    }
}