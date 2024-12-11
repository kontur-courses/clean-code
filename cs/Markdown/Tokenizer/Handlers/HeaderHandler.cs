using Markdown.Tokenizer.Tags;

namespace Markdown.Tokenizer.Handlers;

public class HeaderHandler : IHandler
{
	public Token? ProceedSymbol(TokenizerContext ctx)
	{
		var symbol = ctx.Current;

		if (symbol != '#')
			return null;

		if ((ctx.Next == ' ' && (ctx.Previous == '\n' || ctx.Position == 0)) || (ctx.Previous == '\\'))
		{
			ctx.Advance();
			return new HeaderTag();
		}

		return null;
	}
}