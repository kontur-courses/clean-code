using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer;

/// <summary>
/// Интерфейс токенайзера - переводчика строки в токены
/// </summary>
public interface ITokenizer
{
    public List<IToken> Tokenize(string content);
}