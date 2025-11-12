using FluentAssertions;
using NUnit.Framework;
using Markdown.Data;

namespace Markdown.Tests;

[TestFixture]
class TokenParser_Tests
{
    private void ParseText(string actualInput, Token[] expectedTokens)
    {
        var parser = new TokenParser();
        var actualTokens = parser.ParseTokens(actualInput);
        var act = () => Md.GenerateHtml(actualInput, actualTokens);
        var actualHtml = Md.GenerateHtml(actualInput, actualTokens);
        var expectedHtml = Md.GenerateHtml(actualInput, expectedTokens);
        
        act.Should().NotThrow();
        actualHtml.Should().Be(expectedHtml);
        actualTokens.Should().BeEquivalentTo(expectedTokens);
    }
    
    #region basic tests        
    public static IEnumerable<TestCaseData> ParseSimpleText_Source()
    {
        yield return new TestCaseData(
            "__main title__\n__some bold text__",
            new Token[]
            {
                new (Marks.Bold, 0, 12),
                new (Marks.Bold, 15, 31)
            }).SetName("Bold text");
        yield return new TestCaseData(
            "_main title_\n_some italic text_",
            new Token[]
            {
                new (Marks.Italic, 0, 11),
                new (Marks.Italic, 13, 30)
            }).SetName("Italic text");
        yield return new TestCaseData(
            "# main title\n# some header text",
            new Token[]
            {
                new (Marks.Header, 0, 12),
                new (Marks.Header, 13, 31)
            }).SetName("Headers text");
        yield return new TestCaseData(
            "- main title\n- some bold text",
            new Token[]
            {
                new (Marks.List, 0, 12),
                new (Marks.List, 13, 29)
            }).SetName("List text");
        yield return new TestCaseData(
            "- __bold__\n- _italic_\n- # header",
            new Token[]
            {
                new (Marks.List, 0, 10),
                new (Marks.Bold, 2, 8),
                new (Marks.List, 11, 21),
                new (Marks.Italic, 13, 20),
                new (Marks.List, 22, 32),
                new (Marks.Header, 24, 32)
            }
        ).SetName("List with tags inside");
        yield return new TestCaseData(
            "# - 123\n",
            new Token[]
            {
                new (Marks.Header, 0, 7),
                new (Marks.List, 2, 7),
            }
        ).SetName("List inside header");
    }

    [Test, TestCaseSource(nameof(ParseSimpleText_Source))]
    public void ParseText_OnSimpleText(string actualInput, Token[] expectedTokens)
    {
        ParseText(actualInput, expectedTokens);
    }
        
    public static IEnumerable<TestCaseData> ParseNestingText_Source()
    {
        yield return new TestCaseData(
            "# __main title__\n__some bold text__",
            new Token[]
            {
                new (Marks.Header, 0, 16),
                new (Marks.Bold, 2, 14),
                new (Marks.Bold, 17, 33)
            }).SetName("Inside header");
        yield return new TestCaseData(
            "# __main title__\n# __some _bold_ text__",
            new Token[]
            {
                new (Marks.Header, 0, 16),
                new (Marks.Bold, 2, 14),
                new (Marks.Header, 17, 39),
                new (Marks.Bold, 19, 37),
                new (Marks.Italic, 26, 31)
            }).SetName("Italic inside Bold");
    }
        
    [Test, TestCaseSource(nameof(ParseNestingText_Source))]
    public void ParseText_OnNestingText(string actualInput, Token[] expectedTokens)
    {
        ParseText(actualInput, expectedTokens);
    }
    # endregion    
    
    public static IEnumerable<TestCaseData> ParseTextWithExceptions_Source()
    {
        yield return new TestCaseData(
            "внутри _одинарного __двойное__ не_ работает",
            new Token[]
            {
                new (Marks.Italic, 7, 33)
            }).SetName("Bold inside Italic");
        yield return new TestCaseData(
            "c цифрами_12_3 не считаются выделением __даже1так__",
            new Token[] { }
        ).SetName("Numbers aren't tagged");
        yield return new TestCaseData(
            "_нач_ало се_ред_ина ко_нец_",
            new Token[]
            {
                new (Marks.Italic, 0, 4),
                new (Marks.Italic, 11, 15),
                new (Marks.Italic, 22, 26),
            }
        ).SetName("Parts of words");
        yield return new TestCaseData(
            "эти__ подчерки__ не считаются и эти __подчерки __не считаются",
            new Token[] { }
        ).SetName("Spaces after/before bold mark");
        yield return new TestCaseData(
            "эти_ подчерки_ не считаются и эти _подчерки _не считаются",
            new Token[] { }
        ).SetName("Spaces after/before italic mark");
        yield return new TestCaseData(
            "выделение в ра_зных сл_овах н__е работ__ает",
            new Token[] { }
        ).SetName("Words splitted");
        yield return new TestCaseData(
            "__Непарные символы в рамках одного абзаца не считаются выделением",
            new Token[] { }
        ).SetName("Use only pairs of marks");
        yield return new TestCaseData(
            "пустая строка ____",
            new Token[] { }
        ).SetName("Empty text inside tags");
        yield return new TestCaseData(
            "__пересечения _двойных__ и одинарных_",
            new Token[] { }
        ).SetName("Crossing marks from example");
        yield return new TestCaseData(
            "__пересечения _двойных__ и __одинарных_ подчерков__",
            new Token[]
            {
                new (Marks.Bold, 0, 49),
                new (Marks.Italic, 14, 38)
            }
        ).SetName("Crossing marks");
        yield return new TestCaseData(
            "_пересечения __двойных_ и _одинарных__ подчерков_",
            new Token[] { }
        ).SetName("Crossing marks reversed");
        yield return new TestCaseData(
            @"экран\_ирование\_ и \\__двойное\\__ экрани\рование",
            new Token[]
            {
                new (Marks.Bold,22, 33)
            }
        ).SetName("Shielding marks");
    }
        
    [Test, TestCaseSource(nameof(ParseTextWithExceptions_Source))]
    public void ParseText_WithExceptions(string actualInput, Token[] expectedTokens)
    {
        ParseText(actualInput, expectedTokens);
    }
}