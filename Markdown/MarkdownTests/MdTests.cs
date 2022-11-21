using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MdTests
{
	[SetUp]
	public void SetUp()
	{
		markdown = new Md();
	}

	private Md markdown;

	[TestCase("__Bold__", "<strong>Bold</strong>", TestName = "Bold tag")]
	[TestCase("_Italic_", "<em>Italic</em>", TestName = "Italic tag")]
	[TestCase("# Header", "<h1>Header</h1>", TestName = "Top level header tag")]
	public void Render_ShouldReturnHTMLText_WithCorrectMdText(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}
}