using NUnit.Framework;

namespace Markdown.Tests.Markdown;

[TestFixture]
public class MarkdownTests
{
	private IMarkdownRenderer renderer;

	[SetUp]
	public void SetUp()
	{
		renderer = new MarkdownRenderer();
	}

	[TestCaseSource(nameof(MarkdownRendererTestCases))]
	public string MarkdownRenderer_Verify(string input) => renderer.Render(input);

	private static TestCaseData[] MarkdownRendererTestCases =
	[
		new TestCaseData("# Header").Returns("<h1>Header</h1>").SetDescription("Простой заголовок."),
		new TestCaseData("\\# Header").Returns("# Header").SetDescription("Экранированный заголовок."),
		new TestCaseData("\\\\# Header").Returns("\\<h1>Header</h1>").SetDescription("Экранирован экранирования."),
		new TestCaseData("_Italic text_").Returns("<em>Italic text</em>").SetDescription("Курсив"),
		new TestCaseData("\\_Text_").Returns("_Text_").SetDescription("Экранирование курсива."),
		new TestCaseData("\\\\_Italic text_").Returns("\\<em>Italic text</em>")
			.SetDescription("Экранирование экранирования курсива."),
		new TestCaseData("_Italic text").Returns("_Italic text").SetDescription("Одинокий открывающий тэг."),
		new TestCaseData("Italic text_").Returns("Italic text_").SetDescription("Одинокий закрывающий тэг."),
		new TestCaseData("Italic_ text_").Returns("Italic_ text_").SetDescription("Два закрывающих тэга."),
		new TestCaseData("_Italic _text").Returns("_Italic _text").SetDescription("Два открывающих тэга."),
		new TestCaseData("_нач_але").Returns("<em>нач</em>але").SetDescription("Курсив в начале слова."),
		new TestCaseData("сер_еди_не").Returns("сер<em>еди</em>не").SetDescription("Курсив в середине слова."),
		new TestCaseData("кон_це._").Returns("кон<em>це.</em>").SetDescription("Курсив в конце слова."),
		new TestCaseData("цифры_1_12_3").Returns("цифры_1_12_3").SetDescription("Между цифр - подчерки."),
		new TestCaseData("в ра_зных сл_овах не").Returns("в ра_зных сл_овах не")
			.SetDescription("В разных словах - не работает."),
		new TestCaseData("__bold__").Returns("<strong>bold</strong>").SetDescription("Полужирный"),
		new TestCaseData("_Text__").Returns("_Text__").SetDescription("Разные тэги 1"),
		new TestCaseData("__Text_").Returns("__Text_").SetDescription("Разные тэги 2"),
		new TestCaseData("__Italic __text").Returns("__Italic __text").SetDescription("Два открывающих тэга."),
		new TestCaseData("__два _один_ может__").Returns("<strong>два <em>один</em> может</strong>")
			.SetDescription("Курсив в полужирном."),
		new TestCaseData("_одинарного __двойное__ не_").Returns("<em>одинарного __двойное__ не</em>")
			.SetDescription("Полужирный в курсиве - не работает."),
	];
}