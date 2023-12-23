using Markdown.Filters;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers;

public class MarkdownParser : IMarkingParser
{
    public IList<IToken> ParseText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<IToken> { new Token("", TokenType.Text, 0) };
        }

        var splitter = new TextSplitter();
        var splittedTokens = splitter.SplitOnTokens(text);
        var tagsToChangeInItalic = new HashSet<TagType> { TagType.Strong };
        var tagsToChangeInLink = new HashSet<TagType>(SupportedTags.Tags.Values.Select(t => t.TagType));

        var filters = CreateFilters(text, (TagType.Italic, tagsToChangeInItalic), (TagType.Link, tagsToChangeInLink));
        IList<IToken> filtredTokens = splittedTokens.ToList();
        foreach (var filter in filters.OrderBy(f => f.Order))
            filtredTokens = filter.Filter(filtredTokens);

        return filtredTokens;
    }

    private List<IFilter> CreateFilters(string text, params (TagType, HashSet<TagType>)[] tagsToChange)
    {
        var filters = new List<IFilter>
        {
            new EscapedTagsFilter(),
            new IncorrectTagsFilter(text),
            new NonPairTokensFilter(text),
            new StrongTagsFilter(text),
        }; 
        foreach (var tagToChange in tagsToChange)
            filters.Add(new NestedTokensFilter(tagToChange.Item1, tagToChange.Item2));

        return filters;
    }
}
