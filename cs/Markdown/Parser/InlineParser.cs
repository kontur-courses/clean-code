using Markdown.Node.Interfaces;
using Markdown.Parser.Interfaces;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser;

public class InlineParser : IInlineParser
{
    private readonly UnderscoreTagMatcher _underscoreMatcher;
    private readonly LinkMatcher _linkMatcher;

    public InlineParser(UnderscoreTagMatcher underscoreMatcher, LinkMatcher linkMatcher)
    {
        _underscoreMatcher = underscoreMatcher;
        _linkMatcher = linkMatcher;
    }

    public IReadOnlyList<INode> Parse(IReadOnlyList<Token> tokens)
    {
        throw new NotImplementedException();
    }
}