using FluentAssertions;
using NUnit.Framework;

namespace Markdown;

[TestFixture]
public class MdTests
{
	[TestCase("_окруженный с двух сторон_", "<em>окруженный с двух сторон</em>")]
	public void SingleUnderscore_Should_RendersToEm(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("__Выделенный двумя символами текст__", "<strong>Выделенный двумя символами текст</strong>")]
	public void DoubleUnderscore_Should_RendersToStrong(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase(@"\_Вот это\_", "_Вот это_")]
	[TestCase(@"Здесь сим\волы экранирования\ \должны остаться.\", @"Здесь сим\волы экранирования\ \должны остаться.\")]
	[TestCase(@"\\_вот это будет выделено тегом_", @"<em>вот это будет выделено тегом</em>")]
	public void Backslash_Should_EscapingKeySymbols(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase(
		"Внутри __двойного выделения _одинарное_ тоже__ работает",
		"Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает")]
	public void SingleUnderscoreInDoubleUnderscore_Should_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("внутри _одинарного __двойное__ не_ работает.", "внутри <em>одинарного __двойное__ не</em> работает.")]
	public void DoubleUnderscoreInSingleUnderscore_ShouldNot_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase(
	"Подчерки внутри текста c цифрами_12_3 не считаются выделением",
	"Подчерки внутри текста c цифрами_12_3 не считаются выделением")]
	public void UnderscoreInWordWithDigits_ShouldNot_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("_нач_але", "<em>нач</em>але")]
	[TestCase("сер_еди_не", "сер<em>еди</em>не")]
	[TestCase("кон_це._", "кон<em>це.</em>")]
	public void UnderscoreInSameWord_Should_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("в ра_зных сл_овах не работает", "в ра_зных сл_овах не работает")]
	public void UnderscoreInDifferentWords_ShouldNot_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("__Непарные_ символы", "__Непарные_ символы")]
	public void UnpairSymbolsWithinParagraph_ShouldNot_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("эти_ подчерки_ не считаются выделением", "эти_ подчерки_ не считаются выделением")]
	[TestCase("эти _подчерки_ считаются выделением", "эти <em>подчерки</em> считаются выделением")]
	public void OpeningUnderscorePrecededBySpace_Should_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	// Подмумать над тестами. В спецификации непонятное поведение
	[TestCase("эти _подчерки _не считаются_ окончанием выделения", "эти _подчерки <em>не считаются</em> окончанием выделения")]
	public void ClosingUnderscoreFollowedBySpace_Should_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_")]
	public void SingleUnderscoreAndDoubleUnderscoreIntersection_ShouldNot_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("пустая строка ____", "пустая строка ____")]
	public void EmptyUnderscore_ShouldNot_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("# Заголовки", "<h1>Заголовки</h1>")]
	[TestCase("#Заголовки", "#Заголовки")]
	public void NumberSignFollowedBySpace_Should_RendersToH1(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
	public void HeadersWithDifferentSymbols_Should_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	private void CheckRendering(string text, string expected)
	{
		var md = new Md();

		var html = md.Render(text);

		html.Should().Be(expected);
	}
}
