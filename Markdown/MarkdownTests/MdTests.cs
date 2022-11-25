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
	public void Render_ShouldReturnHTMLText_WithCorrectMdText(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}

	[TestCase(
		"выделение в ра_зных сл_овах не работает",
		"выделение в ра_зных сл_овах не работает",
		TestName = "выделение в разных словах")]
	[TestCase(
		"__tags _inter__ sect_ ions",
		"__tags _inter__ sect_ ions",
		TestName = "Tags intersections")]
	public void Render_ShouldNotRender_WithIncorrectMdTags(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}

	[TestCase("_abc_def", "<em>abc</em>def")]
	[TestCase("a_bcde_f", "a<em>bc>de</em>f")]
	[TestCase("abc_def_", "abc<em>def</em>")]
	[TestCase("__abc__def", "<strong>abc</strong>def")]
	[TestCase("a__bcde__f", "a<strong>bc>de</strong>f")]
	[TestCase("abc__def__", "abc<strong>def</strong>")]
	public void Render_ShouldRender_WithTagsInPartOfWord(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}

	[TestCase(" _1abc1_ ", " _1abc1_ ")]
	public void Render_ShouldNotRender_WithDigitsInTags(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}

	[TestCase(
		"Внутри __двойного выделения _одинарное_ тоже__ работает",
		"Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает",
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
		"Внутри _одинарного выделения __двойное__ не_ работает",
		"Внутри <em>одинарного выделения __двойное__ не</em> работает",
		TestName = "Bold can't be nesting in italic")]
	public void Render_ShouldNotRenderTags_WithIncorrectNestingTags(string mdText, string expected)
	{
		var result = markdown.Render(mdText);

		result.Should().Be(expected);
	}
}