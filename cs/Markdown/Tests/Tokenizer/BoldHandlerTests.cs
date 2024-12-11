using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Handlers;
using Markdown.Tokenizer.Tags;
using Markdown.TreeBuilder;
using NUnit.Framework;

namespace Markdown.Tests.Tokenizer;

[TestFixture]
public class BoldHandlerTests
{
	private ITokenizer tokenizer;

	[SetUp]
	public void SetUp()
	{
		var handlers = new List<IHandler> { new HeaderHandler(), new ItalicHandler(), new BoldHandler() };
		tokenizer = new MarkdownTokenizer(new HandlerManager(handlers), new TagProcessor());
	}

	[TestCaseSource(nameof(BoldTokenSource))]
	public void BoldTokenizerTests((string input, Token[] tags) testCase)
	{
		var tokens = tokenizer.Tokenize(testCase.input).ToArray();

		for (var i = 0; i < testCase.tags.Length; i++)
		{
			tokens[i].Value.Should().Be(testCase.tags[i].Value);
			tokens[i].TokenType.Should().Be(testCase.tags[i].TokenType);
		}
	}

	public static IEnumerable<(string input, Token[] result)> BoldTokenSource()
	{
		yield return ("__abc__", [
			new BoldTag(TagStatus.Open),
			new TextToken("abc"),
			new BoldTag(TagStatus.Closed)
		]);

		yield return ("_abc__", [
			new ItalicTag(TagStatus.Open),
			new TextToken("abc"),
			new BoldTag(TagStatus.Closed)
		]);

		yield return ("__abc_", [
			new BoldTag(TagStatus.Open),
			new TextToken("abc"),
			new ItalicTag(TagStatus.Closed)
		]);
	}
}