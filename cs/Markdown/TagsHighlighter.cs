using Markdown.Tags;

namespace Markdown;

public class TagsHighlighter(IEnumerable<ITag> tags)
{
    public string MarkdownText { get; set; }

    public HighlightedData HighlightMdTags(string markdownText)
    {
        MarkdownText = markdownText;
        return new HighlightedData(markdownText, SingleTagsIndexes(), PairTagsIndexes());
    }

    public Dictionary<Type, List<int>> SingleTagsIndexes()
    {
        var singleTagsIndexes = new Dictionary<Type, List<int>>();

        foreach (var tag in tags.Where(tag => tag.GetType().GetInterface(nameof(ISingleTag)) != null))
        {
            var tagInfos = new List<int>();

            FindSingleTagIndexes(tag, ref tagInfos);

            singleTagsIndexes.Add(tag.GetType(), tagInfos);
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

            if (MarkdownText.IsShielded(tagIdx))
                continue;

            if (MarkdownText.IsSingleTag(tag, tagIdx))
            {
                tagInfos.Add(tagIdx);
                i = tagIdx;
            }
        }
    }

    public Dictionary<Type, List<PairTagInfo>> PairTagsIndexes()
    {
        var pairTagsIndexes = new Dictionary<Type, List<PairTagInfo>>();

        foreach (var tag in tags.Where(tag => tag.GetType().GetInterface(nameof(IPairTag)) != null))
        {
            var tagInfos = new List<PairTagInfo>();

            FindPairTagIndexes(tag, ref tagInfos);

            pairTagsIndexes.Add(tag.GetType(), tagInfos);
        }

        if (tags.Any(tag => tag.GetType().GetInterface(nameof(EmTag)) != null
                            && tag.GetType().GetInterface(nameof(StrongTag)) != null))
        {
            RemoveIntersectStrongAndEmTags(ref pairTagsIndexes);
            RemoveStrongInsideEmTags(ref pairTagsIndexes);
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

            if (MarkdownText.IsShielded(tagIdx))
                continue;

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

                    tagInfos.Add(new PairTagInfo(openIdx, tagIdx));
                    openIdx = -1;
                }

                i = tagIdx;
            }
        }
    }

    public void RemoveIntersectStrongAndEmTags(ref Dictionary<Type, List<PairTagInfo>> pairTagsIndexes)
    {
        var emTagInfos = pairTagsIndexes[typeof(EmTag)];
        var strongTagInfos = pairTagsIndexes[typeof(StrongTag)];

        var i = 0;
        var j = 0;
        while (true)
        {
            if (i >= emTagInfos.Count) break;
            if (j >= strongTagInfos.Count) break;

            var emTagInfo = emTagInfos[i];
            var strongTagInfo = strongTagInfos[j];

            if (emTagInfo.OpenIdx > strongTagInfo.OpenIdx && emTagInfo.OpenIdx < strongTagInfo.CloseIdx &&
                emTagInfo.CloseIdx > strongTagInfo.CloseIdx)
            {
                emTagInfos.RemoveAt(i);
                strongTagInfos.RemoveAt(j);
                continue;
            }

            if (strongTagInfo.OpenIdx > emTagInfo.OpenIdx && strongTagInfo.OpenIdx < emTagInfo.CloseIdx &&
                strongTagInfo.CloseIdx > emTagInfo.CloseIdx)
            {
                emTagInfos.RemoveAt(i);
                strongTagInfos.RemoveAt(j);
                continue;
            }

            if (emTagInfo.OpenIdx < strongTagInfo.OpenIdx) i++;
            else j++;
        }
    }

    public void RemoveStrongInsideEmTags(ref Dictionary<Type, List<PairTagInfo>> pairTagsIndexes)
    {
        var emTagInfos = pairTagsIndexes[typeof(EmTag)];
        var strongTagInfos = pairTagsIndexes[typeof(StrongTag)];

        var i = 0;
        var j = 0;
        while (true)
        {
            if (i >= emTagInfos.Count) break;
            if (j >= strongTagInfos.Count) break;

            var emTagInfo = emTagInfos[i];
            var strongTagInfo = strongTagInfos[j];

            if (emTagInfo.OpenIdx < strongTagInfo.OpenIdx && emTagInfo.CloseIdx > strongTagInfo.CloseIdx)
            {
                strongTagInfos.RemoveAt(j);
                continue;
            }

            if (emTagInfo.OpenIdx < strongTagInfo.OpenIdx) i++;
            else j++;
        }
    }
}