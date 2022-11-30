using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Markdown;
using Markdown.Parsing;
using Markdown.Parsing.Tags;
using Markdown.Render;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownParserItalicTagTests
{
    private MarkdownParser? _mdParser;

    [SetUp]
    public void Setup()
    {
        var italicTag = new ItalicMdTag();

        _mdParser = new MarkdownParser(new List<IMdTag>()
        {
            italicTag
        });
    }


    [TestCase("_testfs gdfgsd fgвфыв df_", "_testfs gdfgsd fgвфыв df_", TestName = "Последовательность слов, разделенных пробелами")]
    [TestCase("_testfgdf_uljkl", "_testfgdf_", TestName = "Тег в начале и в середине слова")]
    [TestCase("testfgdf_uljkl_", "_uljkl_", TestName = "Тег в середине и в конце слова")]
    [TestCase("test_fgdf_uljkl", "_fgdf_", TestName = "Тег в середине слова")]
    public void Parse_ItalicTag_RightMatch(string sourceText, string matchText)
    {
        var matches = _mdParser!.ParseToMatches(sourceText);

        using (new AssertionScope())
        {
            matches.Count.Should().Be(1);
            matches[0].Text.Should().Be(matchText);
        }
    }

    [TestCase("test_qwerty123123_qwerty", TestName = "Цифры внутри тегов _ в одном слове")]
    [TestCase("test_qwerty123 тест13тест qwe_asdf", TestName = "Цифры внутри тегов _ в разных словах")]
    public void Parse_ItalicTagWithNums_NotRightMatch(string sourceText)
    {
        var matches = _mdParser!.ParseToMatches(sourceText);
        matches.Count.Should().Be(0);
    }


    [TestCase("test_asdf\rasdad_qwerty", TestName = "Теги на разных строках(абзацах) разделенных \\r")]
    [TestCase("test_asdf\nasdad_qwerty", TestName = "Теги на разных строках(абзацах) разделенных \\n")]
    [TestCase("test_asdf\r\nasdad_qwerty", TestName = "Теги на разных строках(абзацах) разделенных \\r\\n")]
    public void Parse_ItalicTagOnDifferentLines_NotRightMatch(string sourceText)
    {
        var matches = _mdParser!.ParseToMatches(sourceText);
        matches.Count.Should().Be(0);
    }

    [TestCase("__Непарные_ символы в рамках одного абзаца не считаются выделением", TestName = "Непарные начальные символы в рамках одного абзаца не считаются выделением")]
    [TestCase("_Непарные__ символы в рамках одного абзаца не считаются выделением", TestName = "Непарные конечные символы в рамках одного абзаца не считаются выделением")]
    public void Parse_ItalicTagNotPairTags_NotRightMatch(string sourceText)
    {
        var matches = _mdParser!.ParseToMatches(sourceText);
        matches.Count.Should().Be(0);
    }

    [Test]
    public void Parse_ItalicThreeTags_MatchFirst()
    {
        var matches = _mdParser!.ParseToMatches("_testfgdf_uljkl__");

        using (new AssertionScope())
        {
            matches.Count.Should().Be(1);
            matches[0].Text.Should().Be("_testfgdf_");
        }
    }

    [Test]
    public void Parse_SpaceAfterStartItalicTag_NotRightMatch()
    {
        var matches = _mdParser!.ParseToMatches("_ testfgdf_");

        matches.Count.Should().Be(0);
    }

    [Test]
    public void Parse_SpaceBeforeEndItalicTag_NotRightMatch()
    {
        var matches = _mdParser!.ParseToMatches("_testfgdf _");

        matches.Count.Should().Be(0);
    }

    [Test]
    public void Parse_ItalicTagsInMiddleOfDifferentWords_NotRightMatch()
    {
        var matches = _mdParser!.ParseToMatches("В то же время выделение в ра_зных сл_овах не работает.");

        matches.Count.Should().Be(0);
    }

    [Test]
    public void Parse_ItalicTagsWithEmptyString_NotRightMatch()
    {
        var matches = _mdParser!.ParseToMatches("__");

        matches.Count.Should().Be(0);
    }
}