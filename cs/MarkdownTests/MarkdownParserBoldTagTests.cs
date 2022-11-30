using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Markdown.Parsing;
using Markdown.Parsing.Tags;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownParserBoldTagTests
{
    private MarkdownParser? _mdParser;

    [SetUp]
    public void Setup()
    {
        var boldTag = new BoldMdTag();
        var italicTag = new ItalicMdTag();

        _mdParser = new MarkdownParser(new List<IMdTag>()
        {
            boldTag,
            italicTag
        });
    }


    [TestCase("__testfs gdfgsd fgвфыв df__", "__testfs gdfgsd fgвфыв df__", TestName = "Последовательность слов, разделенных пробелами")]
    [TestCase("__test1__", "__test1__", TestName = "Последовательность слов без пробелов")]
    public void Parse_BoldTag_RightMatch(string sourceText, string matchText)
    {
        var matches = _mdParser!.ParseToMatches(sourceText);

        using (new AssertionScope())
        {
            matches.Count.Should().Be(1);
            matches[0].Text.Should().Be(matchText);
        }
    }

    [TestCase("__testfgdf__uljkl", TestName = "Тег в начале и в середине слова")]
    [TestCase("testfgdf__uljkl__", TestName = "Тег в середине и в конце слова")]
    [TestCase("test__fgdf__uljkl", TestName = "Тег в середине слова")]
    public void Parse_BoldTag_NotRightMatch(string sourceText)
    {
        var matches = _mdParser!.ParseToMatches(sourceText);

        matches.Count.Should().Be(0);
    }


    [TestCase("__qwerty123123__", TestName = "Цифры внутри тегов _ в одном слове")]
    [TestCase("__qwerty123 тест13тест qwe__", TestName = "Цифры внутри тегов _ в разных словах")]
    public void Parse_BoldTagWithNums_RightMatch(string sourceText)
    {
        var matches = _mdParser!.ParseToMatches(sourceText);
        matches.Count.Should().Be(1);
    }

    [TestCase("__test\rqwerty__", TestName = "Теги на разных строках(абзацах) разделенных \\r")]
    [TestCase("__test\nqwerty__", TestName = "Теги на разных строках(абзацах) разделенных \\n")]
    [TestCase("__test\r\nqwerty__", TestName = "Теги на разных строках(абзацах) разделенных \\r\\n")]
    public void Parse_BoldTagOnDifferentLines_NotRightMatch(string sourceText)
    {
        var matches = _mdParser!.ParseToMatches(sourceText);
        matches.Count.Should().Be(0);
    }


    [Test]
    public void Parse_BoldThreeTags_MatchFirst()
    {
        var matches = _mdParser!.ParseToMatches("__testfgdf__uljkl__");

        using (new AssertionScope())
        {
            matches.Count.Should().Be(1);
            matches[0].Text.Should().Be("__testfgdf__uljkl__");
        }
    }

    [Test]
    public void Parse_SpaceAfterStartBoldTag_NotRightMatch()
    {
        var matches = _mdParser!.ParseToMatches("__ testfgdf__");

        matches.Count.Should().Be(0);
    }

    [Test]
    public void Parse_SpaceBeforeEndBoldTag_NotRightMatch()
    {
        var matches = _mdParser!.ParseToMatches("__testfgdf __");

        matches.Count.Should().Be(0);
    }

    [Test]
    public void Parse_BoldTagsInMiddleOfDifferentWords_NotRightMatch()
    {
        var matches = _mdParser!.ParseToMatches("__В то же время выделе__ние в разных словах не работает.");

        matches.Count.Should().Be(0);
    }

    [Test]
    public void Parse_ItalicTagInsideBold_NotRightMatch()
    {
        var matches = _mdParser!.ParseToMatches("__test2_asdasd_test1__");

        using (new AssertionScope())
        {
            matches.Count.Should().Be(1);
            matches[0].Text.Should().Be("_____");
        }
    }

    [Test]
    public void Parse_BoldTagsWithEmptyString_NotRightMatch()
    {
        var matches = _mdParser!.ParseToMatches("____");

        matches.Count.Should().Be(0);
    }
}