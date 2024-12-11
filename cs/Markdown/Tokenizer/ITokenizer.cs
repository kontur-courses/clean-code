using Markdown.Tokenizer.Tags;

namespace Markdown.Tokenizer;

public interface ITokenizer
{
	List<Token> Tokenize(string text);
}