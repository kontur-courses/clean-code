using FluentAssertions;
using Markdown;
using Markdown.Tokens;

namespace MarkdownTests;

public class TokenHighlighterTests
{
    // [Test]
    // public void FindStrongTagIndexes()
    // {
    //     var actual = new TokenHighlighter().HighlightTokens(
    //         "___a__ __b__as __a__ c__"
    //     );
    //
    //     var expected = new List<IToken>
    //     {
    //         new StrongToken("___a__"),
    //         new StringToken(""),
    //         new StrongToken("__b__as __a__"),
    //         new StringToken(" c__")
    //     };
    //
    //     actual.Should().BeEquivalentTo(expected);
    // }

    [Test]
    public void FindCorrectHeaderPosition()
    {
        var actual = new TokenHighlighter().HighlightTokens(
            "asfsf asf gewg e\t #asxf # \n# a"
        );

        var expected = new List<IToken>
        {
            new StringToken("asfsf asf gewg e\t #asxf # \n"),
            new HeaderToken("# a"),
        };

        actual.Should().BeEquivalentTo(expected);
    }

    // [Test]
    // public void IgnoreTagWithShielding()
    // {
    //     var actual = new TokenHighlighter().HighlightTokens("\\_a_");
    //
    //     actual.Should().BeEmpty();
    // }
    //
    // [Test]
    // public void IgnoreIntersectionStrongAndEmTags()
    // {
    //     var actual = new TokenHighlighter().HighlightTokens(
    //         "__пересечения _двойных__ и одинарных_"
    //     );
    //
    //     var expected = new List<IToken>
    //     {
    //         new StrongToken("___a__"),
    //         new StringToken(""),
    //         new StrongToken("__b__as __a__"),
    //         new StringToken(" c__")
    //     };
    //
    //     actual.Should().BeEquivalentTo(expected);
    // }
    //
    // [Test]
    // public void IgnoreStrongInsideEmTag()
    // {
    //     var actual = new TokenHighlighter().HighlightTokens(
    //         "внутри _одинарного __двойное__ не_ работает"
    //     );
    //
    //     var expected = new List<IToken>
    //     {
    //         new StrongToken("___a__"),
    //         new StringToken(""),
    //         new StrongToken("__b__as __a__"),
    //         new StringToken(" c__")
    //     };
    //
    //     actual.Should().BeEquivalentTo(expected);
    // }
}