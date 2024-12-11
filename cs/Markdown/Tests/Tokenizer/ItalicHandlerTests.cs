using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Handlers;
using Markdown.Tokenizer.Tags;
using Markdown.TreeBuilder;
using NUnit.Framework;


namespace Markdown.Tests.Tokenizer;

[TestFixture]
public class ItalicParserTests
{
	private ITokenizer tokenizer;

	[SetUp]
	public void SetUp()
	{
		var handlers = new List<IHandler> { new HeaderHandler(), new ItalicHandler(), new BoldHandler() };
		tokenizer = new MarkdownTokenizer(new HandlerManager(handlers), new TagProcessor());
	}

	[TestCaseSource(nameof(ItalicTokenSource))]
	public void ItalicTokenizerTests((string input, Token[] tags) testCase)
	{
		var tokens = tokenizer.Tokenize(testCase.input).ToArray();

		for (var i = 0; i < testCase.tags.Length; i++)
		{
			tokens[i].Value.Should().Be(testCase.tags[i].Value);
			tokens[i].TokenType.Should().Be(testCase.tags[i].TokenType);
		}
	}

	private static IEnumerable<(string input, Token[] tags)> ItalicTokenSource()
	{
		yield return ("abc", [new TextToken("abc")]);
		yield return ("_abc", [new ItalicTag(TagStatus.Open), new TextToken("abc")]);
		yield return ("abc_", [new TextToken("abc"), new ItalicTag(TagStatus.Closed)]);
		yield return ("a_bc_", [
			new TextToken("a"),
			new ItalicTag(TagStatus.InWord),
			new TextToken("bc"),
			new ItalicTag(TagStatus.Closed)
		]);
		yield return ("_a_bc", [
			new ItalicTag(TagStatus.Open),
			new TextToken("a"),
			new ItalicTag(TagStatus.InWord),
			new TextToken("bc")
		]);

		yield return ("_a_bc_", [
			new ItalicTag(TagStatus.Open),
			new TextToken("a"),
			new ItalicTag(TagStatus.InWord),
			new TextToken("bc"),
			new ItalicTag(TagStatus.Closed)
		]);

		yield return ("_abc_", [
			new ItalicTag(TagStatus.Open),
			new TextToken("abc"),
			new ItalicTag(TagStatus.Closed)
		]);

		yield return ("\\_abc", [
			new SlashToken(),
			new ItalicTag(TagStatus.Open),
			new TextToken("abc")
		]);

		yield return ("\\\\_abc", [
			new SlashToken(),
			new SlashToken(),
			new ItalicTag(TagStatus.Open),
			new TextToken("abc")
		]);
	}
}