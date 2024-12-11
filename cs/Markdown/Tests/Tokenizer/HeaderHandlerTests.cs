using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Handlers;
using Markdown.Tokenizer.Tags;
using Markdown.TreeBuilder;
using NUnit.Framework;

namespace Markdown.Tests.Tokenizer;

[TestFixture]
public class HeaderHandlerTests
{
	private ITokenizer tokenizer;

	[SetUp]
	public void SetUp()
	{
		var handlers = new List<IHandler> { new HeaderHandler(), new ItalicHandler(), new BoldHandler() };
		tokenizer = new MarkdownTokenizer(new HandlerManager(handlers), new TagProcessor());
	}

	[TestCaseSource(nameof(HeaderTokenSource))]
	public void HeaderTokenizerTests((string input, Token[] tags) testCase)
	{
		var tokens = tokenizer.Tokenize(testCase.input).ToArray();

		for (var i = 0; i < testCase.tags.Length; i++)
		{
			tokens[i].Value.Should().Be(testCase.tags[i].Value);
			tokens[i].TokenType.Should().Be(testCase.tags[i].TokenType);
		}
	}

	private static IEnumerable<(string input, Token[] tags)> HeaderTokenSource()
	{
		yield return ("abc", [new TextToken("abc")]);
		yield return ("# abc", [new HeaderTag(), new TextToken("abc")]);
		yield return ("f# abc", [new TextToken("f#"), new TextToken(" abc")]);
		yield return ("\\# abc", [new SlashToken(), new HeaderTag(), new TextToken("abc")]);
		yield return ("\\\\# abc", [new SlashToken(), new SlashToken(), new HeaderTag(), new TextToken("abc")]);
		yield return ("# abc\n# qwe", [
			new HeaderTag(),
			new TextToken("abc"),
			new NewLineToken(),
			new HeaderTag(),
			new TextToken("qwe")
		]);
	}
}