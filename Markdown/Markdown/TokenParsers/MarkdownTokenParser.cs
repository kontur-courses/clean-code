using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParsers;

public class MarkdownTokenParser : ITokenParser
{
	private readonly Dictionary<MarkdownTag, TokenType> tokenTypes;

	public MarkdownTokenParser()
	{
		tokenTypes = new Dictionary<MarkdownTag, TokenType>
		{
			{ new MarkdownTag("_"), TokenType.Italic },
			{ new MarkdownTag("__"), TokenType.Bold },
			{ new MarkdownTag("# ", $"{Environment.NewLine}{Environment.NewLine}"), TokenType.Header }
			//{ new MarkdownTag(string.Empty, string.Empty), TokenType.PlainText}
		};
	}

	public IToken Parse(string text)
	{
		var paragraphs = text.Split($"{Environment.NewLine}{Environment.NewLine}");
		var result = new Token(null, TokenType.PlainText);

		foreach (var paragraph in paragraphs)
		{
			var token = ParseParagraph(paragraph);
			result.nextToken = token;
		}

		return result;
	}

	private IToken ParseParagraph(string paragraph)
	{
		throw new NotImplementedException();
	}
}