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
			{ TokenType.PlainText, new HtmlTag(string.Empty, string.Empty) }
		};
	}

	public string Render(IToken token)
	{
		var htmlTextBuilder = new StringBuilder();

		while (token != null)
		{
			var tag = htmlTags[token.Type];

			var renderedToken = RenderToken(token);
			htmlTextBuilder.Append($"{tag.WithStart}{renderedToken.Value}{tag.WithEnd}");

			token = token.nextToken;
		}

		return htmlTextBuilder.ToString();
	}

	private IToken RenderToken(IToken token)
	{
		if (token.nestingTokens != null) /*return string.Empty;*/ throw new NotImplementedException();

		return token;
	}
}