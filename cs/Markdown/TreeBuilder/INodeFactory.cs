using Markdown.Tokenizer.Tags;

namespace Markdown.TreeBuilder;

public interface INodeFactory
{
	NodeAction? CreateNode(Token token);
}