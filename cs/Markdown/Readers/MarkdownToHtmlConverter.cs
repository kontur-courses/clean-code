using System.Collections.Frozen;
using System.Text;
using Markdown.Converters;
using Markdown.Converters.Base;
using Markdown.Extensions;
using Markdown.Helpers;
using Markdown.Models;

namespace Markdown.Readers;

public class MarkdownToHtmlConverter
{
    private readonly HashSet<IConverter> converters =
    [
        new ItalicConverter(),
        new BoldConverter(),
        new HeaderConverter(),
        new BulletedListConverter(),
    ];

    public string ConvertMarkdownToHtml(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        var tokens = new List<TagPair>();
        foreach (var converter in converters)
        {
            tokens.AddRange(converter.Convert(text).ToList());
        }

        var converterTokenPairs = TagPairHelpers.FilterNonIntersectingTagPairs(tokens);
        var replacementTagPairs = FilterBoldTokens(converterTokenPairs);
        var replacementTokens = TagPairHelpers.FilterShielding(replacementTagPairs, text).ToList();
        return RestoreToHtmlText(replacementTokens, text);
    }

    private IReadOnlyCollection<TagPair> FilterBoldTokens(IReadOnlyCollection<TagPair> tokenPairs)
    {
        var convertTokens = tokenPairs
            .GroupBy(x => x.ConverterType)
            .ToDictionary(x => x.Key, IReadOnlyCollection<TagPair> (x) => x.ToList());
        if (!convertTokens.ContainsKey(typeof(BoldConverter))
            || !convertTokens.ContainsKey(typeof(ItalicConverter)))
        {
            return tokenPairs;
        }

        var boldTokens = convertTokens[typeof(BoldConverter)];
        var italicTokens = convertTokens[typeof(ItalicConverter)];
        convertTokens[typeof(BoldConverter)] = boldTokens.FilterOverlapping(italicTokens);
        var boldFilterTokens = convertTokens.SelectMany(x => x.Value).ToList();
        return boldFilterTokens;
    }

    private string RestoreToHtmlText(IReadOnlyCollection<TagToken> converterTokens, string originalText)
    {
        var replacements = converterTokens.OrderBy(r => r.TagIndex);
        var resultBuilder = new StringBuilder();
        var lastIndex = 0;

        foreach (var token in replacements)
        {
            if (token.TagIndex > lastIndex)
            {
                resultBuilder.Append(originalText.AsSpan(lastIndex, token.TagIndex - lastIndex));
            }
        
            resultBuilder.Append(token.ReplaceTag);
            lastIndex = token.TagIndex + token.SearchTag.Length >= originalText.Length 
                ? originalText.Length
                : token.TagIndex + token.SearchTag.Length;
        }

        if (lastIndex < originalText.Length)
        {
            resultBuilder.Append(originalText.AsSpan(lastIndex));
        }

        return resultBuilder.ToString();
    }
}