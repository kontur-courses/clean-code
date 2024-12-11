using Markdown.Tokenizer.Tags;

namespace Markdown.Tokenizer.Handlers;

public interface IHandler
{
	Token? ProceedSymbol(TokenizerContext context);
}