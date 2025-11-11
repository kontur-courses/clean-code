using Markdown.Dto;

namespace Markdown.TokensUtils.Abstractions;

public interface ITokenizer
{
    public IEnumerable<Token> Tokenize(string? line);
}