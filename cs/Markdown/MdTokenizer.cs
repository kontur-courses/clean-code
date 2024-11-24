using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public class MdTokenizer
{
    private readonly SortedDictionary<string, Func<IToken>> registeredTokens;
    private readonly string text;

    public MdTokenizer(string text, IEnumerable<ITag> tags)
    {
        throw new NotImplementedException();
    }

    public LinkedList<IToken> Tokenize()
    {
        throw new NotImplementedException();
    }
}