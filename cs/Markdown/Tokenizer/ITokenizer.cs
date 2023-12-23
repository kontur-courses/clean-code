using Markdown.Tokens;

namespace Markdown.Tokenizer;

public interface ITokenizer
{
    public IEnumerable<Token> Tokenize(string str);
}