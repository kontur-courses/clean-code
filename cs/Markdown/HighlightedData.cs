using System.Collections.ObjectModel;

namespace Markdown;

public class HighlightedData(string markdownText, IDictionary<Type, List<PairTagInfo>> tagsIndexes)
{
    public string MarkdownText { get; } = markdownText;
    public ReadOnlyDictionary<Type, List<PairTagInfo>> TagsIndexes => tagsIndexes.AsReadOnly();
}