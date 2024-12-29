using Markdown.Tags;
using Markdown.Tags.TagsInteraction;

namespace Markdown;

public class MdParser(string markdownText, IReadOnlyDictionary<char, Dictionary<string, Func<string, int, Tag>>> tagCreators)
{
    private class ReaderPosition
    {
        public int Position { get; set; }
    }

    public Tag[] GetTags()
    {
        var lines = markdownText.Split('\n');
        var result = new List<Tag>();
        foreach (var line in lines)
        {
            var tokens = Tokenize(markdownText);
            var possibleTags = FindCorrectTags(tokens);
            var possibleTagsStarts = possibleTags.Select(t => t.TagStart).ToHashSet();
            var current = new ReaderPosition();
            var tagsIterator = possibleTags.OrderBy(t => t.TagStart).GetEnumerator();
            while (tagsIterator.MoveNext())
            {
                var newTag = CreateTag(current, [], tagsIterator, possibleTagsStarts);
                if (newTag.IsTagClosed)
                    result.Add(newTag);
            }
        }
        
        return result.ToArray();
    }

    private Tag CreateTag(ReaderPosition current, List<Tag> external, IEnumerator<Tag> tagsIterator, HashSet<int> tagsStarts)
    {
        var openTag = tagsIterator.Current;
        current.Position = openTag.SkipTag(openTag.TagStart);
        var isContextCorrect = openTag.AcceptIfContextCorrect(current.Position);
        var nested = new List<Tag>();
        while (current.Position < markdownText.Length && !openTag.AcceptIfContextEnd(current.Position))
        {
            if (tagsStarts.Contains(current.Position))
            {
                if (tagsIterator.MoveNext())
                {
                    var newTag = CreateTag(current, [openTag], tagsIterator, tagsStarts);
                    if (newTag.IsTagClosed)
                    {
                        nested.Add(newTag);
                    }
                }
            }
            else
            {
                isContextCorrect = isContextCorrect && openTag.AcceptIfContextCorrect(current.Position);
                current.Position++;
            }
        }

        if ((external.Count == 0 || Tag.IsNestedTagWorks(external[^1].TagType, openTag.TagType)) && isContextCorrect)
        {
            openTag.TryCloseTag(current.Position, markdownText, out var tagEnd, nested);
            current.Position = tagEnd;
        }
        else
        {
            current.Position = openTag is PairTag ? openTag.SkipTag(current.Position) : current.Position;
        }

        return openTag;
    }

    private Tag GetOpenTag(int tagStart, string markdownText, out int contextStart)
    {
        var tagBegin = markdownText[tagStart];
        var tags = tagCreators[tagBegin];
        var orderedTags = tags.Keys.OrderByDescending(k => k.Length);
        foreach (var tag in orderedTags)
        {
            if (tag.Length + tagStart <= markdownText.Length
                && markdownText.Substring(tagStart, tag.Length) == tag)
            {
                contextStart = tagStart + tag.Length;
                var createOpenTag = tags[tag];
                return createOpenTag(markdownText, tagStart);
            }
        }
        throw new InvalidOperationException("Попытка получить несуществующий тег");
    }

    public List<Tag> Tokenize(string markdownText)
    {
        var tokens = new List<Tag>();
        for (int i = 0; i < markdownText.Length; i++)
        {
            if (tagCreators.ContainsKey(markdownText[i]))
            {
                var tag = GetOpenTag(i, markdownText, out var contextStart);
                tokens.Add(tag);
                i = contextStart - 1;
                if (tag is Escape) i++;
            }
        }
        return tokens;
    }

    public List<Tag> FindCorrectTags(List<Tag> tokens)
    {
        var result = MatchTags(tokens);
        var closedTags = result.Closed;
        var unclosedPairTag = result.Unclosed;
        closedTags.AddRange(new UnclosedPairTagsRules().Apply(unclosedPairTag.Reverse().ToList()));
        return closedTags;
    }

    public (List<Tag> Closed, Stack<Tag> Unclosed) MatchTags(List<Tag> possibleTags)
    {
        var closedTags = new List<Tag>();
        var stack = new Stack<Tag>();

        for (int i = 0; i < possibleTags.Count; i++)
        {
            var tag = possibleTags[i];
            if (tag is not PairTag)
            {
                closedTags.Add(tag);
            }
            else if (stack.Count == 0 || stack.Peek().TagType != tag.TagType)
            {
                stack.Push(tag);
            }
            else
            {
                var startTag = stack.Pop();

                closedTags.Add(startTag);
            }
        }
        return (closedTags, stack);
    }
}