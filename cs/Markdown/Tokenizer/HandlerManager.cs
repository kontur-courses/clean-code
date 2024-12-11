using System.Text;
using Markdown.Tokenizer.Handlers;
using Markdown.Tokenizer.Tags;

namespace Markdown.Tokenizer;

public class HandlerManager(IEnumerable<IHandler> handlers) : IHandlerManager
{
	private readonly List<IHandler> handlers = handlers.ToList();

	public void TryHandle(TokenizerContext context, StringBuilder buffer, List<Token> tags, Stack<Token> tagStack)
	{
		foreach (var handler in handlers)
		{
			var tag = handler.ProceedSymbol(context);
			if (tag != null)
			{
				if (buffer.Length > 0)
				{
					var token = new TextToken(buffer.ToString());
					tags.Add(token);
					buffer.Clear();
				}

				tags.Add(tag);
				tagStack.Push(tag);
				return;
			}
		}

		buffer.Append(context.Current);
	}
}