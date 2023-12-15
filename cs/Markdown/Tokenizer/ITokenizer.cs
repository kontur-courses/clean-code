using Markdown.Tokens;

namespace Markdown.Tokenizer;

public interface ITokenizer
{
    public IEnumerable<IToken> Tokenize(string str);
}