using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class EmphasisInteractionTests : MdTestBase
{
    [Test]
    public void Render_EmInsideStrong_Works()
        => Render("Внутри __двойного выделения _одинарное_ тоже__ работает.")
            .Should().Be("Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.");

    [Test]
    public void Render_StrongInsideEm_DoesNotWork()
        => Render("внутри _одинарного __двойное__ не_ работает")
            .Should().Be("внутри <em>одинарного __двойное__ не</em> работает");

    [Test]
    public void Render_UnderscoresInsideTextWithDigits_AreNotEmphasis()
        => Render("текста c цифрами_12_3 не").Should().Be("текста c цифрами_12_3 не");

    [Test]
    public void Render_EmphasisInsideOneWord_Works()
        => Render("в _нач_але, и в сер_еди_не, и в кон_це._")
            .Should().Be("в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>");

    [Test]
    public void Render_EmphasisAcrossDifferentWords_DoesNotWork()
        => Render("в ра_зных сл_овах не").Should().Be("в ра_зных сл_овах не");

    [TestCase("__Непарные_ символы")]
    [TestCase("_a")]
    [TestCase("a_")]
    [TestCase("__a")]
    public void Render_UnpairedUnderscores_AreNotEmphasis(string input)
        => Render(input).Should().Be(input);

    [Test]
    public void Render_OpeningUnderscoreFollowedBySpace_IsNotEmphasis()
        => Render("эти_ подчерки_ не").Should().Be("эти_ подчерки_ не");

    [Test]
    public void Render_ClosingUnderscorePrecededBySpace_IsNotEmphasis()
        => Render("эти _подчерки _не считаются").Should().Be("эти _подчерки _не считаются");

    [TestCase("__пересечения _двойных__ и одинарных_")]
    [TestCase("_пересечения __одинарных_ и двойных__")]
    public void Render_CrossingUnderscores_AreNotEmphasis(string input)
        => Render(input).Should().Be(input);

    [TestCase("пустая ____ строка")]
    [TestCase("пустая __ __ строка")]
    public void Render_EmptyEmphasis_StaysAsUnderscores(string input)
        => Render(input).Should().Be(input);
}
