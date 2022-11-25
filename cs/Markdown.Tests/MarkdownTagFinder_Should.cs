using FluentAssertions;
using Markdown.Finder;
using Markdown.Tags;

namespace Markdown.Tests;

public class MarkdownTagFinder_Should
{
    private MarkdownTagFinder markdownFinder;

    [SetUp]
    public void Setup()
    {
        markdownFinder = new MarkdownTagFinder();
    }

    [TestCase("____")]
    [TestCase("_ ____")]
    public void FindTags_ShouldNotCount_EmptyTags(string line)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Should().BeEmpty();
    }


    [TestCase("string_123_ num")]
    [TestCase("Digit string__123__")]
    public void FindTags_ShouldNotCount_TaggedDigitsInsideWords(string line)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Should().BeEmpty();
    }

    [TestCase("__123__")]
    [TestCase("_123_")]
    public void FindTags_ShouldCount_WordsWithOnlyDigits(string line)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Should().NotBeEmpty();
    }

    [TestCase("__Bold__", 1, 7)]
    [TestCase("text with __bold__ word", 11, 17)]
    public void FindTags_ShouldFind_SimpleBoldTags(string line, int start, int end)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.First().Should().BeEquivalentTo(new Tag(new TagPosition(start, end), TagType.Bold));
    }

    [TestCase("_B__o__ld_", 0, 9)]
    [TestCase("text _with __bold__ word_", 5, 24)]
    public void FindTags_ShouldNotCount_BoldInsideItalic(string line, int start, int end)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.First().Should().BeEquivalentTo(new Tag(new TagPosition(start, end), TagType.Italic));
        htmlText.Count.Should().Be(1);
    }

    [TestCase("__B_o_ld__", 2)]
    [TestCase("text __with _italic_ word__", 2)]
    [TestCase("text __with _italic_ _italic_ _italic_ word__", 4)]
    public void FindTags_ShouldCount_ItalicInsideBold(string line, int count)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Count.Should().Be(count);
    }

    [Test]
    public void FindTags_ShouldNotFind_TagsInDifferentWords()
    {
        var htmlText = markdownFinder.FindTags("Bo__ld wo__rd").ToList();
        htmlText.Should().BeEmpty();
    }

    [TestCase("_Ita_", 0, 4)]
    [TestCase("text with _Ita_ word", 10, 14)]
    public void FindTags_ShouldFind_SimpleItalicTags(string line, int start, int end)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.First().Should().BeEquivalentTo(new Tag(new TagPosition(start, end), TagType.Italic));
    }

    [TestCase("_It__a_ a__")]
    [TestCase("__It_a__a_")]
    public void FindTags_ShouldNotFind_IntersectingTags(string line)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Should().BeEmpty();
    }

    [Test]
    public void FindTags_ShouldAdd_EscapeTags()
    {
        var htmlText = markdownFinder.FindTags("\\_It_").ToList();
        htmlText.First().Should().BeEquivalentTo(new Tag(new TagPosition(0, 0), TagType.EscapedSymbol));
    }

    [TestCase("\\\\_вот_")]
    public void FindTags_ShouldIgnore_EscapeSymbolWithEscapeSymbol(string line)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Last().Should().BeEquivalentTo(new Tag(new TagPosition(2, 6), TagType.Italic));
    }


    [TestCase("\\вот")]
    [TestCase("в\\о\\т")]
    public void FindTags_ShouldNotCount_EscapeSymbolWithNotShieldedSymbol(string line)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Should().BeEmpty();
    }

    [TestCase("__unpaired_")]
    [TestCase("_unpaired__")]
    [TestCase("_unpaired")]
    [TestCase("unpaired__")]
    public void FindTags_ShouldIgnore_UnpairedTags(string line)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Should().BeEmpty();
    }


    [TestCase("_very_long__word__", 2)]
    [TestCase("_un_paired", 1)]
    [TestCase("unpa__ired__", 1)]
    public void FindTags_ShouldFindTags_InOneWord(string line, int count)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Count.Should().Be(count);
    }

    [TestCase("__word __", 0)]
    [TestCase("__word __word__", 1)]
    public void FindTags_ShouldIgnore_WhiteSpacedTag(string line, int count)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Count.Should().Be(count);
    }

    [TestCase("# _sequence_ with __a __lot__ of t_a_gs __word_1_ _word2_ _word3_ da__", 7)]
    public void FindTags_ShouldWorkCorrect_WithALotOfTags(string line, int count)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Count.Should().Be(count);
    }

    [TestCase("# header")]
    [TestCase("# header__with__tag")]
    [TestCase("# header__with__ _two_ tags")]
    public void FindTags_ShouldFind_HeaderTag(string line)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.First().Should().BeEquivalentTo(new Tag(new TagPosition(0, line.Length - 1), TagType.Header));
    }

    [TestCase("_ABC __QWERTY__", TagType.Bold, 1)]
    [TestCase("_ABC __QWERTY__ __manyTags__", TagType.Bold, 2)]
    public void FindTags_ShouldFind_BoldTagsWhenItalicNotClosed(string line, TagType type, int count)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.Count.Should().Be(count);
        htmlText.All(tag => tag.Type == type).Should().BeTrue();
    }

    [TestCase("* ABC QWERTY", 1)]
    [TestCase("* _ABC_", 2)]
    [TestCase("* __QWERTY__", 2)]
    [TestCase("* _ABC_ __QWERTY__", 3)]
    public void FindTags_ShouldFind_MarkedListTags(string line, int count)
    {
        var htmlText = markdownFinder.FindTags(line).ToList();
        htmlText.First().Type.Should().Be(TagType.MarkedListItem);
        htmlText.Count.Should().Be(count);
    }

    [Test]
    public void FindTags_ShouldWorkCorrect_InExampleLine()
    {
        var htmlText = markdownFinder.FindTags("# Заголовок __с _разными_ символами__").ToList();
        htmlText.Should().BeEquivalentTo(new List<Tag>
        {
            new(new TagPosition(0, 36), TagType.Header),
            new(new TagPosition(13, 36), TagType.Bold),
            new(new TagPosition(16, 24), TagType.Italic)
        });
    }
}