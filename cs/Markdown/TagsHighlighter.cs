using Markdown.Tags;

namespace Markdown;

public class TagsHighlighter(IEnumerable<ITag> tags)
{
    public string MarkdownText { get; set; }

    public string HighlightMdTags(string markdownText)
    {
        MarkdownText = markdownText;

        throw new NotImplementedException();
    }

    public Dictionary<ISingleTag, List<int>> SingleTagsIndexes()
    {
        var singleTagsIndexes = new Dictionary<ISingleTag, List<int>>();

        foreach (var tag in tags.Where(tag => tag.GetType() == typeof(ISingleTag)))
        {
            var tagInfos = new List<int>();

            FindSingleTagIndexes(tag, ref tagInfos);

            singleTagsIndexes.Add((ISingleTag)tag, tagInfos);
        }

        return singleTagsIndexes;
    }

    public void FindSingleTagIndexes(ITag tag, ref List<int> tagInfos)
    {
        for (var i = 0; i < MarkdownText.Length; i++)
        {
            var tagIdx = MarkdownText.IndexOf(tag.Md, i, StringComparison.Ordinal);
            
            if (tagIdx == -1)
                return;

            if (MarkdownText.IsSingleTag(tag, tagIdx))
            {
                tagInfos.Add(tagIdx);
                i = tagIdx;
            }
        }
    }

    public Dictionary<IPairTag, List<PairTagInfo>> PairTagsIndexes()
    {
        var pairTagsIndexes = new Dictionary<IPairTag, List<PairTagInfo>>();

        foreach (var tag in tags.Where(tag => tag.GetType() == typeof(IPairTag)))
        {
            var tagInfos = new List<PairTagInfo>();

            FindPairTagIndexes(tag, ref tagInfos);

            pairTagsIndexes.Add((IPairTag)tag, tagInfos);
        }

        return pairTagsIndexes;
    }

    public void FindPairTagIndexes(ITag tag, ref List<PairTagInfo> tagInfos)
    {
        var openIdx = -1;
        for (var i = 0; i < MarkdownText.Length; i++)
        {
            var tagIdx = MarkdownText.IndexOf(tag.Md, i, StringComparison.Ordinal);

            if (tagIdx == -1)
                return;

            if (MarkdownText.IsPairTag(tag, tagIdx, openIdx == -1))
            {
                if (openIdx == -1)
                    openIdx = tagIdx;
                else
                {
                    if (tagIdx - openIdx <= tag.Md.Length)
                    {
                        openIdx = -1;
                        continue;
                    }

                    tagInfos.Add(new PairTagInfo((openIdx, tagIdx)));
                    openIdx = -1;
                }

                i = tagIdx;
            }
        }
    }

    private void RemoveIntersectStrongAndEmTags(ref (EmTag, List<PairTagInfo>) emTagInfo,
        ref (StrongTag, List<PairTagInfo>) strongTagInfo)
    {
        throw new NotImplementedException();
    }

    private void RemoveStrongInsideEmTags(ref (EmTag, List<PairTagInfo>) emTagInfo,
        ref (StrongTag, List<PairTagInfo>) pairTagInfo)
    {
        throw new NotImplementedException();
    }
}