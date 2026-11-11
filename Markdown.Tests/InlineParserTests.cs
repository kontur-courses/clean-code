using System.Linq;
using Markdown.Parsing;
using Markdown.Syntax;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class InlineParserTests
{
    private InlineParser parser = null!;

    [SetUp]
    public void SetUp()
    {
        parser = new InlineParser();
    }

    [Test]
    public void Parse_PlainText_ReturnsTextNode()
    {
        AssertNodes(parser.Parse("обычный текст"), Text("обычный текст"));
    }

    [Test]
    public void Parse_EmptyText_ReturnsNoNodes()
    {
        Assert.That(parser.Parse(string.Empty), Is.Empty);
    }

    [Test]
    public void Parse_EmphasisMarkers_ReturnEmphasisNode()
    {
        AssertNodes(parser.Parse("_текст_"), Emphasis(Text("текст")));
    }

    [Test]
    public void Parse_StrongMarkers_ReturnStrongNode()
    {
        AssertNodes(parser.Parse("__текст__"), Strong(Text("текст")));
    }

    [Test]
    public void Parse_StrongEmphasisMarkers_ReturnsNestedNodes()
    {
        AssertNodes(
            parser.Parse("___текст___"), Strong(Emphasis(Text("текст"))));
    }

    [Test]
    public void Parse_EmphasisInsideStrong_IsSupported()
    {
        AssertNodes(
            parser.Parse("__жирный _курсив_ жирный__"),
            Strong(Text("жирный "), Emphasis(Text("курсив")), Text(" жирный")));
    }

    [Test]
    public void Parse_StrongInsideEmphasis_RemainsLiteral()
    {
        var actual = parser.Parse("_курсив __жирный__ курсив_");

        Assert.That(actual, Has.Count.EqualTo(1));
        Assert.That(actual[0], Is.TypeOf<EmphasisNode>());
        AssertLiteralText(((EmphasisNode)actual[0]).Children, "курсив __жирный__ курсив");
    }

    [Test]
    public void Parse_EmphasisCanCoverWordPrefix()
    {
        AssertNodes(parser.Parse("_часть_слова"), Emphasis(Text("часть")), Text("слова"));
    }

    [Test]
    public void Parse_EmphasisCanCoverPartOfWord()
    {
        AssertNodes(
            parser.Parse("до_середина_после"),
            Text("до"), Emphasis(Text("середина")), Text("после"));
    }

    [Test]
    public void Parse_EmphasisCanCoverWordSuffix()
    {
        AssertNodes(parser.Parse("до_часть_"), Text("до"), Emphasis(Text("часть")));
    }

    [TestCase("_ текст_", TestName = "Parse_OpeningMarkerFollowedByWhitespace_RemainsLiteral")]
    [TestCase("_текст _", TestName = "Parse_ClosingMarkerPrecededByWhitespace_RemainsLiteral")]
    [TestCase("число_12_3", TestName = "Parse_UnderscoresInsideNumber_RemainLiteral")]
    [TestCase("до_части между словами после_границы", TestName = "Parse_EmphasisCannotSpanWords")]
    [TestCase("_без пары", TestName = "Parse_UnmatchedOpeningEmphasisMarker_RemainsLiteral")]
    [TestCase("без пары_", TestName = "Parse_UnmatchedClosingEmphasisMarker_RemainsLiteral")]
    [TestCase("__без пары", TestName = "Parse_UnmatchedOpeningStrongMarker_RemainsLiteral")]
    [TestCase("без пары__", TestName = "Parse_UnmatchedClosingStrongMarker_RemainsLiteral")]
    [TestCase("___без пары", TestName = "Parse_UnmatchedOpeningStrongEmphasisMarker_RemainsLiteral")]
    [TestCase("без пары___", TestName = "Parse_UnmatchedClosingStrongEmphasisMarker_RemainsLiteral")]
    [TestCase("__текст_", TestName = "Parse_UnmatchedStrongMarkerWithEmphasis_RemainsLiteral")]
    [TestCase("___текст__", TestName = "Parse_UnmatchedStrongEmphasisMarkerWithStrong_RemainsLiteral")]
    [TestCase("___текст_", TestName = "Parse_UnmatchedStrongEmphasisMarkerWithEmphasis_RemainsLiteral")]
    [TestCase("____", TestName = "Parse_OnlyUnderscores_RemainsLiteral")]
    [TestCase("__жир_кур__сив_", TestName = "Parse_CrossedEmphasisAndStrongMarkers_RemainLiteral")]
    public void Parse_InvalidMarkup_RemainsLiteral(string input)
    {
        AssertLiteralText(parser.Parse(input), input);
    }

    [Test]
    public void Parse_ClosingMarkerAfterWhitespace_RemainsLiteralInsideEmphasis()
    {
        AssertNodes(
            parser.Parse("_текст1 _текст2_"),
            Emphasis(Text("текст1 _текст2")));
    }

    [TestCase(@"\_буквально\_", "_буквально_", TestName = "Parse_EscapedEmphasisMarkers_ArePlainText")]
    [TestCase(@"\_\_жирный\_\_", "__жирный__", TestName = "Parse_EscapedStrongMarkers_ArePlainText")]
    [TestCase(@"\абв", @"\абв", TestName = "Parse_BackslashBeforeLetter_RemainsLiteral")]
    [TestCase(@"текст\ продолжение", @"текст\ продолжение", TestName = "Parse_BackslashBeforeWhitespace_RemainsLiteral")]
    [TestCase(@"текст\!ещё", "текст!ещё", TestName = "Parse_EscapedOrdinaryCharacter_RemovesEscape")]
    public void Parse_EscapedCharacters_ArePlainText(string input, string expected)
    {
        AssertLiteralText(parser.Parse(input), expected);
    }

    [Test]
    public void Parse_EscapedBackslashDoesNotEscapeFollowingMarker()
    {
        AssertNodes(parser.Parse(@"\\_часть_"), Text("\\"), Emphasis(Text("часть")));
    }

    private static void AssertLiteralText(IReadOnlyList<InlineNode> actual, string expected)
    {
        Assert.That(actual.All(node => node is TextNode), Is.True,
            "Ожидалось, что все узлы будут текстовыми, но найден хотя бы один другой тип узла");

        var actualText = string.Concat(actual.Cast<TextNode>().Select(node => node.Text));
        Assert.That(actualText, Is.EqualTo(expected));
    }

    private static void AssertNodes(IReadOnlyList<InlineNode> actual, params InlineNode[] expected)
    {
        Assert.That(actual, Has.Count.EqualTo(expected.Length));

        for (var i = 0; i < expected.Length; i++)
        {
            AssertNode(actual[i], expected[i]);
        }
    }

    /// <summary>
    /// Рекурсивно проверяет структуру AST, сравнивая типы узлов и их содержимое
    /// </summary>
    /// <param name="actual"></param>
    /// <param name="expected"></param>
    private static void AssertNode(InlineNode actual, InlineNode expected)
    {
        switch (expected)
        {
            case TextNode expectedText:
                Assert.That(actual, Is.TypeOf<TextNode>());
                Assert.That(((TextNode)actual).Text, Is.EqualTo(expectedText.Text));
                break;

            case EmphasisNode expectedEmphasis:
                Assert.That(actual, Is.TypeOf<EmphasisNode>());
                AssertNodes(((EmphasisNode)actual).Children, expectedEmphasis.Children.ToArray());
                break;

            case StrongNode expectedStrong:
                Assert.That(actual, Is.TypeOf<StrongNode>());
                AssertNodes(((StrongNode)actual).Children, expectedStrong.Children.ToArray());
                break;

            default:
                Assert.Fail($"Unexpected node type: {expected.GetType().Name}");
                break;
        }
    }

    private static TextNode Text(string text) => new(text);

    private static EmphasisNode Emphasis(params InlineNode[] children) => new(children);

    private static StrongNode Strong(params InlineNode[] children) => new(children);
}
