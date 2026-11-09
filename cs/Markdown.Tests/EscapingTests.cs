using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class EscapingTests : MdTestBase
{
    [Test]
    public void Render_EscapedUnderscores_AreNotEmphasis()
        => Render(@"\_Вот это\_, не").Should().Be("_Вот это_, не");

    [Test]
    public void Render_BackslashBeforeOrdinaryCharacter_Stays()
        => Render(@"Здесь сим\волы экранирования\ \должны остаться.\")
            .Should().Be(@"Здесь сим\волы экранирования\ \должны остаться.\");

    [Test]
    public void Render_EscapedBackslash_DoesNotEscapeNextUnderscore()
        => Render(@"\\_вот это будет выделено тегом_ \<em>")
            .Should().Be(@"\<em>вот это будет выделено тегом</em> \<em>");
}
