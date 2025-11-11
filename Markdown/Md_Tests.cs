using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics;

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
	[TestCase(@"\\_вот это будет выделено тегом_", @"\<em>вот это будет выделено тегом</em>")]
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
	[TestCase("_Непарные__ символы", "_Непарные__ символы")]
	[TestCase("__Непарные\nсимволы__", "__Непарные\nсимволы__")]
	[TestCase("_Непарные\r\nсимволы_", "_Непарные\r\nсимволы_")]
	public void UnpairSymbolsWithinParagraph_ShouldNot_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("эти_ подчерки_ не считаются выделением", "эти_ подчерки_ не считаются выделением")]
	[TestCase("эти _подчерки_ считаются выделением", "эти <em>подчерки</em> считаются выделением")]
	public void OpeningUnderscorePrecededBySpace_Should_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("эти _подчерки _не считаются _окончанием выделения", "эти _подчерки _не считаются _окончанием выделения")]
	[TestCase("эти _подчерки _не считаются_ окончанием выделения", "эти _подчерки <em>не считаются</em> окончанием выделения")]
	public void ClosingUnderscoreFollowedBySpace_Should_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_")]
	[TestCase("_пересечения __двойных_ и одинарных__", "_пересечения __двойных_ и одинарных__")]
	[TestCase("_перес__еч_ения__ двойных и одинарных", "_перес__еч_ения__ двойных и одинарных")]
	public void SingleUnderscoreAndDoubleUnderscoreIntersection_ShouldNot_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("пустая строка ____", "пустая строка ____")]
	[TestCase("пустая строка __", "пустая строка __")]
	[TestCase("пустая__строка", "пустая__строка")]
	public void EmptyUnderscore_ShouldNot_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("# Заголовки", "<h1>Заголовки</h1>")]
	[TestCase("#Заголовки", "#Заголовки")]
	public void NumberSignFollowedBySpace_Should_RendersToH1(string text, string expected)
		=> CheckRendering(text, expected);

	[TestCase("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
	public void HeadersWithDifferentSymbols_Should_Renders(string text, string expected)
		=> CheckRendering(text, expected);

	[Test]
	public void Perfomance_Should_BeAlmostLinear()
	{
		var mdText =
			"""
						# Спецификация языка разметки

			Посмотрите этот файл в сыром виде. Сравните с тем, что показывает github.
			Все совпадения случайны ;)



			# Курсив

			Текст, _окруженный с двух сторон_ одинарными символами подчерка,
			должен помещаться в HTML-тег \<em> вот так:

			Текст, \<em>окруженный с двух сторон\</em> одинарными символами подчерка,
			должен помещаться в HTML-тег \<em>.



			# Полужирный

			__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега \<strong>.



			# Экранирование

			Любой символ можно экранировать, чтобы он не считался частью разметки.
			\_Вот это\_, не должно выделиться тегом \<em>.

			Символ экранирования исчезает из результата, только если экранирует что-то.
			Здесь сим\волы экранирования\ \должны остаться.\

			Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_ \<em>



			# Взаимодействие тегов

			Внутри __двойного выделения _одинарное_ тоже__ работает.

			Но не наоборот — внутри _одинарного __двойное__ не_ работает.

			Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.

			Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._

			В то же время выделение в ра_зных сл_овах не работает.

			__Непарные_ символы в рамках одного абзаца не считаются выделением.

			За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением 
			и остаются просто символами подчерка.

			Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения 
			и остаются просто символами подчерка.

			В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.

			Если внутри подчерков пустая строка ____, то они остаются символами подчерка.



			# Заголовки

			Абзац, начинающийся с "# ", выделяется тегом \<h1> в заголовок.
			В тексте заголовка могут присутствовать все прочие символы разметки с указанными правилами.

			Таким образом

			# Заголовок __с _разными_ символами__

			превратится в:

			\<h1>Заголовок \<strong>с \<em>разными\</em> символами\</strong>\</h1>
			""";

		mdText = string.Join("", Enumerable.Range(0, 30).Select(t => mdText));
		var md = new Md();

		RenderTextNTimes(md, mdText, 10);

		var sw1 = new Stopwatch();
		sw1.Start();
		RenderTextNTimes(md, mdText, 100);
		sw1.Stop();

		var sw2 = new Stopwatch();
		sw2.Start();
		RenderTextNTimes(md, mdText, 1000);
		sw2.Stop();

		(sw2.Elapsed.TotalMilliseconds / sw1.Elapsed.TotalMilliseconds).Should().BeLessThan(10);
	}

	private void RenderTextNTimes(Md md, string mdText, int count)
	{
		for (var i = 0; i < count; i++)
			md.Render(mdText);
	}

	private void CheckRendering(string text, string expected)
	{
		var md = new Md();

		var html = md.Render(text);

		html.Should().Be(expected);
	}
}
