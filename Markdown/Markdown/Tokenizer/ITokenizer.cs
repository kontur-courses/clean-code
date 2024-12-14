namespace Markdown.Tokenizer;

public interface ITokenizer<TTokenType, out TToken>
{
    public IEnumerable<TToken> Tokenize(ReadOnlyMemory<char> input);
}