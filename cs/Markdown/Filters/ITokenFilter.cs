using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Filters;

public interface ITokenFilter
{
    public IList<IToken> ChangeTypeForEscapedTags(IEnumerable<IToken> tokens);
    public IList<IToken> ChangeTypeForIncorrectTags(IList<IToken> tokens, string text);
    public IList<IToken> ChangeTypeForNonPairTokens(IList<IToken> tokens, string text);
    public IList<IToken> CombineStrongTags(IList<IToken> tokens, string text);
    public IList<IToken> ChangeTypeForNestedTokens(IList<IToken> tokens,
        TagType outer, HashSet<TagType> nested, IList<IToken> tagTokens);
}
