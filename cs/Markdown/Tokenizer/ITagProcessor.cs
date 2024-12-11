using Markdown.Tokenizer.Tags;

namespace Markdown.Tokenizer;

public interface ITagProcessor
{
	void Process(List<Token> tags, Stack<Token> tagStack);
}