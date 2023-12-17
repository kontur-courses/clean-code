using Markdown.Tags;

namespace Markdown;

public class TagsHighlighter
{
    private string markdownText;

    public Dictionary<IPairTag, List<PairTagInfo>> PairTagsIndexes => throw new NotImplementedException();
    public Dictionary<IUnpairTag, List<int>> UnpairTagsIndexes => throw new NotImplementedException();

    public string HighlightMdTags(string markdownText)
    {
        this.markdownText = markdownText;

        throw new NotImplementedException();
    }

    private void RemoveIntersectStrongAndEmTags(ref (EmTag, List<PairTagInfo>) emTagInfo,
        ref (StrongTag, List<PairTagInfo>) strongTagInfo)
    {
        throw new NotImplementedException();
    }

    private void RemoveStrongInsideEmTags(ref (StrongTag, List<PairTagInfo>) pairTagInfo)
    {
        throw new NotImplementedException();
    }
}