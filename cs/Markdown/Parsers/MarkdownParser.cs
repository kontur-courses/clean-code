using Markdown.Filters;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers;

public class MarkdownParser : IMarkingParser
{
    private int currentPosition;
    private ITokenFilter filter;

    public MarkdownParser(ITokenFilter filter)
    {
        this.filter = filter;
    }

    public IList<IToken> ParseText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<IToken> { new Token("", TokenType.Text, 0) };
        }
        var splitter = new TextSplitter(currentPosition);
        var splitedTokens = splitter.SplitOnTokens(text);
        var tokens = filter.ChangeTypeForEscapedTags(splitedTokens);
        tokens = filter.ChangeTypeForIncorrectTags(tokens, text);
        tokens = filter.ChangeTypeForNonPairTokens(tokens, text);
        tokens = filter.CombineStrongTags(tokens, text);
        var tagsToChange = new HashSet<TagType> { TagType.Strong };
        tokens = filter.ChangeTypeForNestedTokens(tokens, TagType.Italic, tagsToChange, GetAllTagTokens(tokens));
        tagsToChange = new HashSet<TagType>(SupportedTags.Tags.Values.Select(t => t.TagType));
        tokens = filter.ChangeTypeForNestedTokens(tokens, TagType.Link, tagsToChange, GetAllTagTokens(tokens));
        currentPosition = 0;
        return tokens;
    }

    private IList<IToken> GetAllTagTokens(IEnumerable<IToken> tokens)
    {
        return tokens.Where(token => token.Type == TokenType.Tag).ToList();
    }
}
