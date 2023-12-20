using System.Text;
using Markdown.TagClasses;

namespace Markdown;

public class Parser : IParser
{
    private IEnumerable<Tag> tags;

    public Parser(IEnumerable<Tag> tags)
    {
        this.tags = tags;
    }

    public IEnumerable<MarkdownTagInfo> GetTagsToRender(string markdownText)
    {
        var allTags = FindTags(markdownText);
        var noEscapedTags = RemoveEscapedTags(allTags);
        var renderTags = PairTags(markdownText, noEscapedTags).OrderBy(tagInfo => tagInfo.StartIndex);
        return renderTags;
    }

    public List<MarkdownTagInfo> FindTags(string markdownText)
    {
        var tagsIndexes = new List<MarkdownTagInfo>();
        for (int i = 0; i < markdownText.Length; i++)
        {
            var tag = FindTag(markdownText, i);
            if (tag == null) continue;

            tagsIndexes.Add(tag);
            i = tag.EndIndex;
        }

        return tagsIndexes;
    }

    private List<MarkdownTagInfo> RemoveEscapedTags(List<MarkdownTagInfo> tagsList)
    {
        var newTags = new List<MarkdownTagInfo>(tagsList.Count);
        for (int i = 0; i < tagsList.Count; i++)
        {
            var currentTag = tagsList[i];
            if (currentTag.Tag is not EscapeTag)
            {
                newTags.Add(currentTag);
                continue;
            };
            if (i + 1 > tagsList.Count - 1)
                break;

            var nextTag = tagsList[i + 1];

            if (nextTag.StartIndex != currentTag.StartIndex + 1)
                continue;

            newTags.Add(currentTag);
            i++;
        }
        return newTags;
    }

    private List<MarkdownTagInfo> PairTags(string markdownText, List<MarkdownTagInfo> tagsList)
    {
        var renderTags = new List<MarkdownTagInfo>(tagsList.Count);
        var tagsStack = new Stack<MarkdownTagInfo>();
        for (int i = 0; i < tagsList.Count; i++)
        {
            var currentTagInfo = tagsList[i];
            if (!currentTagInfo.Tag.Model.ShouldHavePair)
            {
                renderTags.Add(currentTagInfo.OpeningVariant());
                continue;
            }
            var tagsInStack = tagsStack.TryPeek(out var lastTagInfo);

            if (tagsInStack
                && lastTagInfo.Tag.CanBePairedWith(markdownText, lastTagInfo.StartIndex,
                                                    currentTagInfo.Tag, currentTagInfo.EndIndex))
            {
                tagsStack.Pop();
                if (currentTagInfo.Tag.CantBeInsideTags(tagsStack.Select(tagInfo => tagInfo.Tag).ToArray()))
                    continue;

                renderTags.Add(lastTagInfo.Tag.Model.TakePairTag ? 
                    new MarkdownTagInfo(currentTagInfo.Tag, lastTagInfo.StartIndex, lastTagInfo.EndIndex).OpeningVariant() 
                    : lastTagInfo.OpeningVariant());
                renderTags.Add(currentTagInfo.Tag.Model.TakePairTag 
                    ? new MarkdownTagInfo(lastTagInfo.Tag, currentTagInfo.StartIndex, currentTagInfo.EndIndex) 
                    : currentTagInfo);
            }

            else if (tagsInStack && currentTagInfo.Tag.IsMarkdownClosing(markdownText, currentTagInfo.EndIndex))
                tagsStack.Pop();

            else if (currentTagInfo.Tag.CanBeOpened(markdownText, currentTagInfo.StartIndex))
                tagsStack.Push(currentTagInfo);
        }
        return renderTags;
    }

    private MarkdownTagInfo FindTag(string markdownText, int i)
    {
        var substringIndex = i;
        var substring = new StringBuilder();
        substring.Append(markdownText[substringIndex]);

        var markdownSubstring = substring.ToString();
        Tag? resultTag = null;
        do
        {
            if (!SubstringIsInTagMarkdown(markdownSubstring))
                break;

            resultTag = FindTagBySubstring(markdownSubstring);
            substringIndex++;
            if (substringIndex > markdownText.Length - 1) break;

            substring.Append(markdownText[substringIndex]);
            markdownSubstring = substring.ToString();
        } while (true);

        var tagInfo = new MarkdownTagInfo(resultTag, i, substringIndex - 1);

        return resultTag != null ? tagInfo : null;
    }

    private bool SubstringIsInTagMarkdown(string substring)
    {
        foreach (var tag in tags)
        {
            if (tag.Model.MarkdownOpening != null && tag.Model.MarkdownOpening.StartsWith(substring)
                || tag.Model.MarkdownClosing != null && tag.Model.MarkdownClosing.StartsWith(substring))
                return true;
        }
        return false;
    }

    private Tag? FindTagBySubstring(string substring)
    {
        var tagByOpening = FindTagByOpening(substring);
        return tagByOpening ?? FindTagByClosing(substring);
    }

    private Tag? FindTagByOpening(string markdownOpening)
    {
        return tags.FirstOrDefault(tag => tag.Model.MarkdownOpening != null && tag.Model.MarkdownOpening == markdownOpening);
    }

    private Tag? FindTagByClosing(string markdownClosing)
    {
        return tags.FirstOrDefault(tag => tag.Model.MarkdownClosing != null && tag.Model.MarkdownClosing == markdownClosing);
    }
}