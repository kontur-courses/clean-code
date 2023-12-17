using Markdown.Tokens;

namespace Markdown.Tokenizer;

public interface ITokenizer
{
    public IEnumerable<Token> Tokenize(string str);
    public IEnumerable<Token> CreateToken(IEnumerable<Token> tokens);
}