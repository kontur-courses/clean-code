namespace MarkdownRenderer.Infrastructure;

public static class StringExtensions
{
    public static string Substring(this string source, Token token) =>
        source.Substring(token.Start, token.Length);

    public static IEnumerable<(string SplitPart, string SplitterAfter)> EnhancedSplit(
        this string source, params string[] splitters
    )
    {
        var splittersStartHash = splitters.Select(splitter => splitter[0]).ToHashSet();
        var partStart = 0;
        for (var i = 0; i < source.Length; i++)
        {
            if (!splittersStartHash.Contains(source[i]))
                continue;

            var splitter = splitters
                .Where(splitter => i + splitter.Length < source.Length)
                .Where(splitter => splitter.Where((c, j) => c != source[i + j]).Any() is false)
                .DefaultIfEmpty()
                .MaxBy(splitter => splitter?.Length ?? 0);

            if (splitter is null)
                continue;

            yield return (source[partStart..i], splitter);
            partStart = i += splitter.Length;
        }

        yield return (source[partStart..], "");
    }
}