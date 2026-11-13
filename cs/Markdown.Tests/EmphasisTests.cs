using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class EmphasisTests : MdTestBase
{
    [TestCase("_a_", "<em>a</em>")]
    [TestCase("_a b_", "<em>a b</em>")]
    [TestCase("Текст, _окруженный_ подчерками", "Текст, <em>окруженный</em> подчерками")]
    public void Render_SingleUnderscores_ProducesEm(string input, string expected)
        => Render(input).Should().Be(expected);

    [TestCase("__a__", "<strong>a</strong>")]
    [TestCase("__a b__ c", "<strong>a b</strong> c")]
    public void Render_DoubleUnderscores_ProducesStrong(string input, string expected)
        => Render(input).Should().Be(expected);

    [TestCase("текст")]
    [TestCase("")]
    public void Render_TextWithoutMarkup_StaysAsIs(string input)
        => Render(input).Should().Be(input);
}
