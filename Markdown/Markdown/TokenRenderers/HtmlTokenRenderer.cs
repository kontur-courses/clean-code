using System.Text;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenRenderers;

public class HtmlTokenRenderer : ITokenRenderer
{
	private readonly Dictionary<TokenType, HtmlTag> htmlTags;

	public HtmlTokenRenderer()
	{
		htmlTags = new Dictionary<TokenType, HtmlTag>
		{
			{ TokenType.Bold, new HtmlTag("strong") },
			{ TokenType.Header, new HtmlTag("h1") },
			{ TokenType.Italic, new HtmlTag("em") },
			{ TokenType.Link, new HtmlTag("<a href=\"", "\">") },
			{ TokenType.PlainText, new HtmlTag(string.Empty, string.Empty) }
		};
	}

	public string Render(IToken token)
	{
		var htmlTextBuilder = new StringBuilder();

		while (token != null)
		{
			htmlTextBuilder.Append(RenderToken(token));

			token = token.nextToken;
		}

		return htmlTextBuilder.ToString();
	}

	private string RenderToken(IToken token)
	{
		if (token.Type == TokenType.Escape) return RenderEscapeToken(token);
		var tag = htmlTags[token.Type];

		var value = token.nestingTokens == null ? token.Value : RenderNestingTokens(token);

		return $"{tag.Open}{value}{tag.Close}";
	}

	private string RenderEscapeToken(IToken token)
	{
		if (token.Value.Length < 2) return "";
		return token.Value.Substring(1, token.Value.Length - 1);
	}

	private string RenderNestingTokens(IToken token)
	{
		var result = new StringBuilder();
		var nestedToken = token.nestingTokens;

		while (nestedToken is not null)
		{
			var value = nestedToken.nestingTokens == null ? nestedToken.Value : RenderNestingTokens(nestedToken);
			//if (nestedToken.Type == TokenType.Escape) return RenderEscapeToken(nestedToken);
			var tag = htmlTags[nestedToken.Type];
			result.Append($"{tag.Open}{value}{tag.Close}");

			nestedToken = nestedToken.nextToken;
		}

		return result.ToString();
	}
}