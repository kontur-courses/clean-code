using FluentAssertions;
using Markdown;

namespace MarkdownUnitTests;

[TestFixture]
public class Tests
{
    [TestCase("", TestName = "Render_EmptyInput_{a}")]
    [TestCase("Обычный текст.", TestName = "Render_PlainText_{a}")]
    public void Render_PreservesPlainText(string input)
    {
        Md.Render(input).Should().Be(input);
    }

    [TestCase("Привет, _мир_!", "Привет, <em>мир</em>!",
        TestName = "Render_Italic_{a}")]
    [TestCase("__жирный текст__", "<strong>жирный текст</strong>",
        TestName = "Render_Bold_{a}")]
    [TestCase("_нач_але", "<em>нач</em>але",
        TestName = "Render_ItalicAtWordStart_{a}")]
    [TestCase("сер_еди_не", "сер<em>еди</em>не",
        TestName = "Render_ItalicInsideWord_{a}")]
    [TestCase("кон_це._", "кон<em>це.</em>",
        TestName = "Render_ItalicAtWordEnd_{a}")]
    public void Render_ConvertsValidEmphasis(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }

    [TestCase("__жирный _курсив_ текст__",
        "<strong>жирный <em>курсив</em> текст</strong>",
        TestName = "Render_ItalicInsideBold_{a}")]
    [TestCase("_курсив __двойное__ внутри_",
        "<em>курсив __двойное__ внутри</em>",
        TestName = "Render_BoldMarkersInsideItalicStayLiteral_{a}")]
    public void Render_RespectsEmphasisNesting(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }
    
    [TestCase("_без пары", TestName = "Render_UnmatchedItalicMarker_{a}")]
    [TestCase("__без пары", TestName = "Render_UnmatchedBoldMarker_{a}")]
    [TestCase("__Непарные_", TestName = "Render_MarkersOfDifferentLengths_{a}")]
    [TestCase("____", TestName = "Render_EmptyEmphasis_{a}")]
    [TestCase("_ текст_", TestName = "Render_SpaceAfterOpeningMarker_{a}")]
    [TestCase("_текст _", TestName = "Render_SpaceBeforeClosingMarker_{a}")]
    [TestCase("ра_зных сл_овах", TestName = "Render_MarkersInsideDifferentWords_{a}")]
    [TestCase("цифрами_12_3", TestName = "Render_UnderscoresInNumericText_{a}")]
    [TestCase("__пересечения _двойных__ и одинарных_",
        TestName = "Render_CrossingEmphasis_{a}")]
    public void Render_PreservesInvalidEmphasis(string input)
    {
        Md.Render(input).Should().Be(input);
    }
    
    [TestCase(@"\_Вот это\_", "_Вот это_",
        TestName = "Render_EscapedUnderscores_{a}")]
    [TestCase(@"\\_курсив_", @"\<em>курсив</em>",
        TestName = "Render_EscapedBackslashBeforeEmphasis_{a}")]
    [TestCase(@"сим\волы", @"сим\волы",
        TestName = "Render_BackslashBeforeLetter_{a}")]
    [TestCase(@"текст\ ", @"текст\ ",
        TestName = "Render_BackslashBeforeSpace_{a}")]
    [TestCase(@"текст\", @"текст\",
        TestName = "Render_TrailingBackslash_{a}")]
    public void Render_HandlesEscapes(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }
    
    [TestCase("# Заголовок", "<h1>Заголовок</h1>",
        TestName = "Render_Heading_{a}")]
    [TestCase("# Заголовок __с _разными_ символами__",
        "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>",
        TestName = "Render_HeadingWithNestedEmphasis_{a}")]
    [TestCase("#Без пробела", "#Без пробела",
        TestName = "Render_HashWithoutSpace_{a}")]
    [TestCase("Текст # внутри", "Текст # внутри",
        TestName = "Render_HashInsideParagraph_{a}")]
    public void Render_RecognizesHeadings(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }
}