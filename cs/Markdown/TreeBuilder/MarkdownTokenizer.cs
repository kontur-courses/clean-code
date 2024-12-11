using System.Text;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tags;
using Token = Markdown.Tokenizer.Tags.Token;

namespace Markdown.TreeBuilder;

public class MarkdownTokenizer(IHandlerManager handlerManager, ITagProcessor tagProcessor) : ITokenizer
{
	private readonly StringBuilder buffer = new();
	private readonly List<Token> tags = new();
	private readonly Stack<Token> tagStack = new();

	public List<Token> Tokenize(string text)
	{
		var context = new TokenizerContext(text);
		while (!context.IsEnd)
		{
			if (TryProceedSpecialSymbol(context)) continue;

			handlerManager.TryHandle(context, buffer, tags, tagStack);

			context.Advance();
		}

		FlushBuffer();

		tagProcessor.Process(tags, tagStack);

		return tags;
	}

	private bool TryProceedSpecialSymbol(TokenizerContext context)
	{
		switch (context.Current)
		{
			case '\n':
			{
				FlushBuffer();
				var token = new NewLineToken();
				tags.Add(token);
				context.Advance();

				return true;
			}
			case ' ':
			{
				if (buffer.Length > 0)
				{
					tags.Add(new TextToken(buffer.ToString()));
					buffer.Clear();
				}

				buffer.Append(context.Current);
				context.Advance();

				return true;
			}
			case '\\':
				FlushBuffer();
				tags.Add(new SlashToken());
				context.Advance();

				return true;
			default:
				return false;
		}
	}

	private void FlushBuffer()
	{
		if (buffer.Length > 0)
		{
			tags.Add(new TextToken(buffer.ToString()));
			buffer.Clear();
		}
	}
}