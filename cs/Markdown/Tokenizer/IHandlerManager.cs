using System.Text;
using Markdown.Tokenizer.Tags;

namespace Markdown.Tokenizer;

public interface IHandlerManager
{
	void TryHandle(TokenizerContext context, StringBuilder buffer, List<Token> tags, Stack<Token> tagStack);
}