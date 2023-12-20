using Markdown.Tags;

namespace Markdown;

public class TagsHighlighter(IEnumerable<ITag> tags)
{
    private string markdownText;

    public TagsHighlighter(IEnumerable<ITag> tags, string markdownText) : this(tags)
    {
        this.markdownText = markdownText;
    }

    public HighlightedData HighlightMdTags(string markdownText)
    {
        this.markdownText = markdownText;
        return new HighlightedData(markdownText, TagsIndexes());
    }

    public Dictionary<Type, List<PairTagInfo>> TagsIndexes()
    {
        var pairTagsIndexes = new Dictionary<Type, List<PairTagInfo>>();

        foreach (var tag in tags)
        {
            var tagInfos = new List<PairTagInfo>();

            FindTagIndexes(tag, ref tagInfos);

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

    public void FindTagIndexes(ITag tag, ref List<PairTagInfo> tagInfos)
    {
        var openIdx = -1;
        for (var i = 0; i < markdownText.Length; i++)
        {
            var tagIdx = markdownText.IndexOf(tag.Md, i, StringComparison.Ordinal);

            if (tagIdx == -1)
                return;

            if (markdownText.IsShielded(tagIdx))
                continue;

            if (markdownText.IsTag(tag, tagIdx, openIdx == -1))
            {
                if (openIdx == -1)
                {
                    openIdx = tagIdx;

                    if (tag.GetType() == typeof(HeaderTag))
                    {
                        tagInfos.Add(new PairTagInfo(openIdx, markdownText.CloseIndexOfParagraph(tagIdx)));
                        openIdx = -1;
                        i = tagIdx;
                        continue;
                    }
                }
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
            }

            i = tagIdx;
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