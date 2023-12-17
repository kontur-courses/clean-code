using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class PairParser_Should
{
    private static Tag[] tags = Tags.GetAllTags().ToArray();
    private PairParser parser;

    [SetUp]
    public void CreateNewSplitterInstance()
    {
        parser = new PairParser(tags);
    }

    [TestCase("_Lorem ipsum dolor_")]
    [TestCase("__Lorem ipsum dolor__")]
    [TestCase("# Lorem ipsum dolor\n")]
    public void ParseTagPairsParsesOneTagPair(string inputString)
    {
        var tagPairs = parser.ParseTagPairs(inputString);
        tagPairs.Should().HaveCount(1);
        var tagPair = tagPairs.First();
        tagPair.FirstTagInfo.StartIndex.Should().Be(0);
        tagPair.SecondTagInfo.StartIndex.Should().Be(17 + tagPair.FirstTagInfo.Tag.MarkdownOpening.Length);
    }

    [Test]
    public void ParseTagPairsSkipsEscapedTags()
    {
        var tagPairs = parser.ParseTagPairs("\\_Lorem_ ipsum _dolor_ sit amet");
        tagPairs.Should().HaveCount(1);
        var tagPair = tagPairs.First();
        tagPair.FirstTagInfo.StartIndex.Should().Be(15);
        tagPair.SecondTagInfo.StartIndex.Should().Be(21);
    }

    [Test]
    public void ParseTagPairsEscapesOnlyTags()
    {
        var tagPairs = parser.ParseTagPairs("_Lorem_ ipsum _do\\lor_ sit amet");
        tagPairs.Should().HaveCount(2);
        var firstPair = tagPairs.First();
        var secondPair = tagPairs.Skip(1).First();
        firstPair.FirstTagInfo.StartIndex.Should().Be(0);
        secondPair.SecondTagInfo.StartIndex.Should().Be(21);
    }

    [Test]
    public void ParseTagPairsEscapesEscapedTags()
    {
        var tagPairs = parser.ParseTagPairs("\\\\_Lorem ipsum_ _dolor_ sit amet");
        tagPairs.Should().HaveCount(2);
        var firstPair = tagPairs.First();
        var secondPair = tagPairs.Skip(1).First();
        firstPair.FirstTagInfo.StartIndex.Should().Be(2);
        secondPair.FirstTagInfo.StartIndex.Should().Be(16);
    }

    [Test]
    public void ParseTagPairsParsesInsideWords()
    {
        var tagPairs = parser.ParseTagPairs("_Lor_em ip_s_um dol_or._");
        tagPairs.Should().HaveCount(3);
        var firstPair = tagPairs.First();
        var secondPair = tagPairs.Skip(1).First();
        var thirdPair = tagPairs.Skip(2).First();
        firstPair.FirstTagInfo.StartIndex.Should().Be(0);
        secondPair.FirstTagInfo.StartIndex.Should().Be(10);
        thirdPair.FirstTagInfo.StartIndex.Should().Be(19);
    }

    [TestCase("Lorem_1_2 123_456_789", TestName = "TagsInsideNumbers")]
    [TestCase("Lor_em ips_um", TestName = "TagPairInSeparateWords")]
    [TestCase("_Lorem __ipsum_ dolor__", TestName = "IntersectingTags")]
    [TestCase("Lorem __ipsum_", TestName = "UnpairedTags")]
    [TestCase("Lorem__ ipsum__", TestName = "WrongOpenedTags")]
    [TestCase("Lorem __ipsum __sit", TestName = "WrongClosedTags")]
    [TestCase("____ __ # \n", TestName = "EmptyTagPair")]
    public void ParseTagPairsSkips(string testingString)
    {
        var tagPairs = parser.ParseTagPairs(testingString);
        tagPairs.Should().HaveCount(0);
    }

    [Test]
    public void ParseTagPairsParsesItalicInBold()
    {
        var tagPairs = parser.ParseTagPairs("Lorem __ipsum _dolor_ sit__");
        tagPairs.Should().HaveCount(2);
    }

    [Test]
    public void ParseTagPairsSkipsBoldInItalic()
    {
        var tagPairs = parser.ParseTagPairs("Lorem _ipsum __dolor__ sit_");
        tagPairs.Should().HaveCount(1);
    }

    [Test]
    public void ParseTagPairsParsesMultiplePairsInHeader()
    {
        var tagPairs = parser.ParseTagPairs("# Lorem __ipsum _sit_ amet__\n");
        tagPairs.Should().HaveCount(3);
        var lastPair = tagPairs.Last();
        lastPair.FirstTagInfo.Tag.Should().Be(Tags.Header);
        lastPair.SecondTagInfo.Tag.Should().Be(Tags.Header);
        lastPair.SecondTagInfo.StartIndex.Should().Be(28);
    }

    [Test]
    [Timeout(1000)]
    public void ParseTagPairsIsLinear()
    {
        var testString = @"# Спецификация языка разметки

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

Абзац, начинающийся с ""# "", выделяется тегом \<h1> в заголовок.
В тексте заголовка могут присутствовать все прочие символы разметки с указанными правилами.

Таким образом

# Заголовок __с _разными_ символами__

превратится в:

\<h1>Заголовок \<strong>с \<em>разными\</em> символами\</strong>\</h1>";

        var tagPairs = parser.ParseTagPairs(testString);
        tagPairs.Should().HaveCount(19);
    }
}