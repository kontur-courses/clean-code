using Markdown.Scanners;
using Markdown.Tokens;

namespace Markdown;

public class MdTokenizer
{
    private ITokenScanner[] scanners = [new SpecScanner(), new TextScanner()];

    public List<Token> Tokenize(string text)
    {
        throw new NotImplementedException();
    }
}