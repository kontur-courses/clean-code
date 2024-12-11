using Markdown.Tokenizer.Tags;

namespace Markdown.Tokenizer.Handlers;

public class ItalicHandler : IHandler
{
	public Token? ProceedSymbol(TokenizerContext ctx)
	{
		var symbol = ctx.Current;

		if (symbol != '_' || ctx.Next == '_')
			return null;

		if (char.IsDigit(ctx.Previous ?? ' ') || char.IsDigit(ctx.Next ?? ' '))
			return null;

		if (ctx.Position == 0 || ctx.Previous == ' ' || ctx.Previous == '\\')
		{
			return new ItalicTag(TagStatus.Open);
		}

		if (ctx.Previous != ' ' && (ctx.Next == ' ' || ctx.Length - 1 == ctx.Position))
		{
			return new ItalicTag(TagStatus.Closed);
		}

		if (ctx.Previous != ' ' && ctx.Next != ' ')
			return new ItalicTag(TagStatus.InWord);

		return null;
	}
}