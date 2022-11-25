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
	[TestCase("aaa __Bold__ aaa", "aaa <strong>Bold</strong> aaa", TestName = "Bold tag in text")]
	[TestCase("aaa _Italic_ aaa", "aaa <em>Italic</em> aaa", TestName = "Italic tag in text")]
	[TestCase("# Header aaa", "<h1>Header aaa</h1>", TestName = "Top level header tag with spaces")]
	[TestCase(
		"# Header aaa\r\n\r\n__Second _para_graph__",
		"<h1>Header aaa</h1>\r\n\r\n<strong>Second <em>para</em>graph</strong>",
		TestName = "Two paragraphs")]
	public void Render_ShouldReturnHTMLText_WithCorrectTags(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}

	[TestCase("/__Bold/__", "__Bold__", TestName = "Bold tag")]
	[TestCase("/_Italic/_", "_Italic_", TestName = "Italic tag")]
	[TestCase("/# Header", "# Header", TestName = "Top level header tag")]
	[TestCase("/_", "_", TestName = "only one italic tag")]
	[TestCase("/__", "__", TestName = "only one bold tag")]
	[TestCase("/# ", "# ", TestName = "only one header tag")]
	public void Render_ShouldNotRenderEscapedTags_WithCorrectTags(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}


	[TestCase(
		"no space _ before_ opening tag",
		"no space _ before_ opening tag",
		TestName = "Space after opening em tag")]
	[TestCase(
		"no space _after _ closing tag",
		"no space _after _ closing tag",
		TestName = "Space before closing em tag")]
	[TestCase(
		"no space __ before__ opening tag",
		"no space __ before__ opening tag",
		TestName = "Space after opening strong tag")]
	[TestCase(
		"no space __after __ closing tag",
		"no space __after __ closing tag",
		TestName = "Space before closing strong tag")]
	[TestCase(
		"Ta_gs beginnin_g or e__nding in d__ifferent words",
		"Ta_gs beginnin_g or e__nding in d__ifferent words",
		TestName = "Tags beginning or ending in different words")]
	[TestCase(
		"__tags _inter__ sect_ ions",
		"__tags _inter__ sect_ ions",
		TestName = "Tags intersections")]
	[TestCase(
		"__",
		"__",
		TestName = "Em tag without value")]
	[TestCase(
		"____",
		"____",
		TestName = "Strong tag without value")]
	[TestCase(
		"abcd # abcd",
		"abcd # abcd",
		TestName = "Header not at the beginning")]
	[TestCase(
		"__Unclosed_ tags",
		"__Unclosed_ tags",
		TestName = "Unclosed tags")]
	public void Render_ShouldNotRender_WithIncorrectTags(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}

	[TestCase("_abc_def", "<em>abc</em>def")]
	[TestCase("a_bcde_f", "a<em>bcde</em>f")]
	[TestCase("abc_def_", "abc<em>def</em>")]
	[TestCase("__abc__def", "<strong>abc</strong>def")]
	[TestCase("a__bcde__f", "a<strong>bcde</strong>f")]
	[TestCase("abc__def__", "abc<strong>def</strong>")]
	public void Render_ShouldRender_WithTagsInPartOfWord(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}

	[TestCase(" _1abc1_ ", " _1abc1_ ")]
	[TestCase(" _a1b1c_ ", " _a1b1c_ ")]
	public void Render_ShouldNotRender_WithDigitsInTags(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}

	[TestCase(
		"Italic __can be _nesting_ in bold__",
		"Italic <strong>can be <em>nesting</em> in bold</strong>",
		TestName = "Italic can be nesting in bold")]
	[TestCase(
		"# Header __can _contain_ nesting__ tags",
		"<h1>Header <strong>can <em>contain</em> nesting</strong> tags</h1>",
		TestName = "Header can contain nesting tags")]
	public void Render_ShouldRenderTags_WithCorrectNestingTags(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}

	[TestCase(
		"aa _bbb c __dddd__ e_ ff",
		"aa <em>bbb c __dddd__ e</em> ff",
		TestName = "Bold can't be nesting in italic")]
	[TestCase(
		"# First header # second header",
		"<h1>First header # second header</h1>",
		TestName = "Header in header")]
	public void Render_ShouldNotRenderTags_WithIncorrectNestingTags(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}
}