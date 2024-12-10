using Markdown.TokenConverters;

namespace Markdown.Renderers;

public static class TagReplacer
{
    public static string SimilarTagsNester(string text)
    {
        foreach (var pair in TokenConverterFactory.GetTagPairs())
        {
            text = text.Replace(pair.Key, pair.Value);
        }
        return text;
    }
}