using System.Collections.ObjectModel;

namespace Markdown;

public class HighlightedData(string markdownText, 
    IDictionary<Type, List<int>> singleTagsIndexes, IDictionary<Type, List<PairTagInfo>> pairTagsIndexes)
{
    public string MarkdownText { get; } = markdownText;
    public ReadOnlyDictionary<Type, List<int>> SingleTagsIndexes => singleTagsIndexes.AsReadOnly();
    public ReadOnlyDictionary<Type, List<PairTagInfo>> PairTagsIndexes => pairTagsIndexes.AsReadOnly();
}