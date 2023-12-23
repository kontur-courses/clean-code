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
        var splitedTokens = splitter.SplitOnTokens(text);
        var tagsToChangeInStrong = new HashSet<TagType> { TagType.Strong };
        var tagsToChangeInLink = new HashSet<TagType>(SupportedTags.Tags.Values.Select(t => t.TagType));

        var nestedLinkTokensFilter = new NestedTokensFilter(null, TagType.Link, tagsToChangeInLink);
        var nestedStrongInItalicTokensFilter = new NestedTokensFilter(nestedLinkTokensFilter,
            TagType.Italic, tagsToChangeInStrong);
        var strongTagFilter = new StrongTagsFilter(nestedStrongInItalicTokensFilter, text);
        var nonPairsTokensFilter = new NonPairTokensFilter(strongTagFilter, text);
        var incorrectTagsFilter = new IncorrectTagsFilter(nonPairsTokensFilter, text);
        var escapedTagsFilter = new EscapedTagsFilter(incorrectTagsFilter);

        return escapedTagsFilter.Filter(splitedTokens.ToList());
    }
}
