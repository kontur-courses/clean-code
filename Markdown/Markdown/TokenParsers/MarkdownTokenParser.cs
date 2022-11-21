using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParsers;

public class MarkdownTokenParser : ITokenParser
{
	private readonly List<MarkdownTag> availableTags;
	private readonly Dictionary<TokenType, MarkdownTag> markdownTags;
	private readonly Dictionary<MarkdownTag, TokenType> tokenTypes;

	public MarkdownTokenParser()
	{
		tokenTypes = new Dictionary<MarkdownTag, TokenType>
		{
			{ new MarkdownTag("__"), TokenType.Bold },
			{ new MarkdownTag("_"), TokenType.Italic },
			{ new MarkdownTag("# ", $"{Environment.NewLine}{Environment.NewLine}"), TokenType.Header }
			//{ new MarkdownTag(string.Empty, string.Empty), TokenType.PlainText}
		};

		markdownTags = tokenTypes.ToDictionary(pair => pair.Value, pair => pair.Key);
		availableTags = tokenTypes.Keys.ToList();
	}

	public IToken Parse(string text)
	{
		var paragraphs = text.Split($"{Environment.NewLine}{Environment.NewLine}");
		var result = new Token(string.Empty, TokenType.PlainText);

		foreach (var paragraph in paragraphs)
		{
			var token = ParseParagraph(paragraph);
			result.nextToken = token;
		}

		return result;
	}

	private IToken ParseParagraph(string paragraph)
	{
		IToken result = new Token(string.Empty, TokenType.PlainText);
		var currentToken = result;

		for (var i = 0; i < paragraph.Length; i++)
		{
			var currentScope = string.Concat(paragraph.Skip(i));
			foreach (var tag in availableTags)
			{
				if (!currentScope.StartsWith(tag.WithStart)) continue;

				var token = ParseTag(currentScope);
				if (token == null) continue;

				currentToken.nextToken = token;
				currentToken = currentToken.nextToken;
				i += token.Value.Length + tag.WithStart.Length + tag.WithEnd.Length;
				break;
			}
		}

		return result;
	}

	private IToken ParseTag(string scope)
	{
		if (scope.StartsWith(markdownTags[TokenType.Header].WithStart))
			return new Token(string.Concat(scope.Skip(markdownTags[TokenType.Header].WithStart.Length)),
				TokenType.Header);

		foreach (var tag in availableTags)
		{
			if (tag == markdownTags[TokenType.Header]) continue;

			if (!scope.StartsWith(tag.WithStart)) continue;

			var value = ParseValue(string.Concat(scope.Skip(tag.WithStart.Length)), tag);
			return new Token(value, tokenTypes[tag]);
		}

		return new Token(scope, TokenType.PlainText);
	}

	private string ParseValue(string scope, MarkdownTag tag)
	{
		for (var i = 0; i < scope.Length; i++)
		{
			var currentScope = string.Concat(scope.Skip(i));
			if (currentScope.StartsWith(tag.WithEnd)) return string.Concat(scope.Take(i));
		}

		return scope;
		throw new NotImplementedException();
	}
}