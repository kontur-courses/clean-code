using Markdown.Tokens;

namespace Markdown.Interfaces;

public interface ITokenizer
{
    public void Init(string line);
    public List<Token> TokenizeLine();
}