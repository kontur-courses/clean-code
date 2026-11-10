using FluentAssertions;
using Markdown.Extensions;
using Markdown.Tokenization.Tokens;

namespace MarkdownTests.TokenResolverTests;

public class TokenResolverTests
{
    [TestCase("_текст_", new[] { PairRole.Open, PairRole.Close })]
    [TestCase("__текст__", new[] { PairRole.Open, PairRole.Close })]
    [TestCase("сер_еди_не", new[] { PairRole.Open, PairRole.Close })]
    [TestCase("ра_зных сл_овах", new[] { PairRole.Text, PairRole.Text })]
    [TestCase("_несколько слов_", new[] { PairRole.Open, PairRole.Close })]
    [TestCase("цифрами_12_3", new[] { PairRole.Text, PairRole.Text })]
    [TestCase("__a _b_ c__", new[] { PairRole.Open, PairRole.Open, PairRole.Close, PairRole.Close })]
    [TestCase("__abcd _efg___", new[] { PairRole.Open, PairRole.Open, PairRole.Close, PairRole.Close })]
    [TestCase("___abc___", new[] { PairRole.Open, PairRole.Open, PairRole.Close, PairRole.Close })]
    [TestCase("__abc___", new[] { PairRole.Open, PairRole.Text, PairRole.Close })]
    [TestCase("_a __b__ c_", new[] { PairRole.Open, PairRole.Text, PairRole.Text, PairRole.Close })]
    [TestCase("__a _b__ c_", new[] { PairRole.Text, PairRole.Text, PairRole.Text, PairRole.Text })]
    [TestCase("____", new[] { PairRole.Text, PairRole.Text })]
    [TestCase("_а_ _б_", new[] { PairRole.Open, PairRole.Close, PairRole.Open, PairRole.Close })]
    [TestCase("_a\r\nb_", new[] { PairRole.Open, PairRole.Close })]
    [TestCase(@"\_текст_", new[] { PairRole.Text })]
    [TestCase(@"\\_a_", new[] { PairRole.Open, PairRole.Close })]
    [TestCase("__[ссылка](https://example.org)__", new[] { PairRole.Open, PairRole.Close })]
    [TestCase("_[__текст_](url)_", new[] { PairRole.Open, PairRole.Close })]
    [TestCase("[__текст_](url)", new PairRole[] { })]
    [TestCase("_текст _", new[] { PairRole.Text, PairRole.Text })]
    [TestCase("а_ текст_", new[] { PairRole.Text, PairRole.Text })]
    public void Resolve_ShouldAssignExpectedRoles(string source,PairRole[] expectedRoles)
    {
        var roles = Resolve(source)
            .OfType<PairToken>()
            .Select(token => token.Context!.Role);

        roles.Should().Equal(expectedRoles);
    }

    [TestCase("_abc_", new[] { 0, 0 })]
    [TestCase("_a_ _b_", new[] { 0, 0, 1, 1 })]
    [TestCase(" _a_ _b_", new[] { 1, 1, 2, 2 })]
    [TestCase("_a_  _b_", new[] { 0, 0, 1, 1 })]
    [TestCase("_a_ \t _b_", new[] { 0, 0, 1, 1 })]
    [TestCase("_a_ _b_ _c_", new[] { 0, 0, 1, 1, 2, 2 })]
    [TestCase("ра_зных сл_овах", new[] { 0, 1 })]
    [TestCase("ра_зных,сл_овах", new[] { 0, 0 })]
    [TestCase("_a\r\nb_", new[] { 0, 0 })]
    [TestCase(@"_a\_b_", new[] { 0, 0 })]
    [TestCase("_[a b](url)_", new[] { 0, 0 })]
    [TestCase("_a [b c](url) d_", new[] { 0, 2 })]
    [TestCase("__a _b_ c__", new[] { 0, 1, 1, 2 })]
    public void Resolve_ShouldAssignCorrectWordSegments(string source, int[] expectedSegments)
    {
        var segments = Resolve(source)
            .OfType<PairToken>()
            .Select(token => token.Context!.SegmentId);

        segments.Should().Equal(expectedSegments);
    }

    private static ResolvedTokens Resolve(string source)
        => source.GetParagraphs().First().Tokenize().Resolve();
}
