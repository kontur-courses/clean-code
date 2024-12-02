using System.Text;
using Markdown.HtmlConverting.HtmlTagConverters;
using Markdown.MdParsing.Interfaces;
using Markdown.Tokens;

namespace Markdown.HtmlConverting;

public class TokensToHtmlConverter : ITokensConverter
{
    private readonly Dictionary<TagType, IHtmlTagConverter> tagConverters = CreateTagConverters();

    private static Dictionary<TagType, IHtmlTagConverter> CreateTagConverters()
    {
        return new List<BaseHtmlTagConverter>
        {
            new HeaderTagConverter(),
            new ItalicTagConverter(),
            new StrongTagConverter(),
            new LinkTagConverter()
        }.ToDictionary<BaseHtmlTagConverter, TagType, IHtmlTagConverter>(
            key => key.HandledTag, 
            value => value);
    }

    public string Convert(IReadOnlyList<Token> tokens)
    {
        var result = new StringBuilder();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (!TokenUtilities.TryGetTagTypeByOpenTag(token, out var tagType))
            {
                result.Append(token.Content);
                continue;
            }
            
            var converter = tagConverters[tagType!.Value];
            var nextPosition = i + 1;
            var htmlText = converter.ConvertTokensToHtmlText(tagConverters,
                tokens,
                nextPosition,
                out var readTokens);
            result.Append(htmlText);
            i += readTokens;
        }
        
        return result.ToString();
    }
}