using Markdown.Tokenizer.Tags;

namespace Markdown.Tokenizer.Handlers;

public class BoldHandler : IHandler
{
	public Token? ProceedSymbol(TokenizerContext ctx)
	{
		var symbol = ctx.Current;

		if (symbol != '_')
			return null;

		if (ctx.Next != '_')
			return null;

		if ((ctx.Position == 0 || ctx.Previous == ' ') && ctx.NextNext != ' ')
		{
			ctx.Advance();
			return new BoldTag(TagStatus.Open);
		}

		if (ctx.Previous != ' ')
		{
			ctx.Advance();
			return new BoldTag(TagStatus.Closed);
		}

		return null;
	}
}