using Markdown;
using FluentAssertions;

namespace MarkdownTests;

public class HeadingTests
{
    private Md md;

    [SetUp]
    public void SetUp()
    {
        md = new Md();
    }
    [Test]
    public void Test_StandartHeading()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("# aa# aa# aa# aa# aa# aa").Should().Be("<h1>aa# aa# aa# aa# aa# aa</h1>");
        md.Render("# aaaa").Should().Be("<h1>aaaa</h1>");
        md.Render("#aaaa").Should().Be("#aaaa");
    }
    [Test]
    public void Test_HeadingTagsWithoutText()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("#\n# Заголовок").Should().Be("\n<h1>Заголовок</h1>");
        md.Render("#").Should().Be("");
        md.Render("##").Should().Be("");
        md.Render("###").Should().Be("");
    }

    [Test]
    public void Test_HeadingWithOtherTags()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("# __a__" + "\n" + "#  _b_").Should().Be("<h1><strong>a</strong></h1>" + '\n' + "<h1><em>b</em></h1>");
        md.Render("# Заголовок __с _разными_ символами__").Should().Be("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>");
        md.Render("# __bold _italic_ text__").Should().Be("<h1><strong>bold <em>italic</em> text</strong></h1>");
        md.Render("# __bold _italic_ text__\n").Should().Be("<h1><strong>bold <em>italic</em> text</strong></h1>\n");
    }

    [Test]
    public void Test_HeadingWithTrailingSpaces()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("#   Заголовок").Should().Be("<h1>Заголовок</h1>");
        md.Render("# Заголовок   ").Should().Be("<h1>Заголовок</h1>");
        md.Render("# Заголовок      ф").Should().Be("<h1>Заголовок ф</h1>");
    }

    [Test]
    public void Test_InvalidHeadingWithoutSpace()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("##Heading").Should().Be("##Heading");
        md.Render("#Heading").Should().Be("#Heading");
    }

    [Test]
    public void Test_HeadingWithEmptyLines()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("# aaa" + '\n' + "#  bbb").Should().Be("<h1>aaa</h1>" + '\n' + "<h1>bbb</h1>");
        md.Render("# aaa bbb" + '\n' + "#  bbb").Should().Be("<h1>aaa bbb</h1>" + '\n' + "<h1>bbb</h1>");
        md.Render("#     aaa" + '\n' + "bbb").Should().Be("<h1>aaa</h1>" + '\n' + "bbb");
        md.Render("# abb" + "\n" + "ba_").Should().Be("<h1>abb</h1>" + "\n" + "ba_");
        md.Render("# Заголовок\n\n# Второй").Should().Be("<h1>Заголовок</h1>\n\n<h1>Второй</h1>");
    }

    [Test]
    public void Test_HeadingWithSpecialCharacters()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("# @#$%^&*() Заголовок").Should().Be("<h1>@#$%^&*() Заголовок</h1>");
        md.Render("# Заголовок с символами !@#").Should().Be("<h1>Заголовок с символами !@#</h1>");
    }

    [Test]
    public void Test_HeadingWithOnlyWhitespace()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("#      ").Should().Be("");
        md.Render("#\t\t").Should().Be("");
    }

    [Test]
    public void Test_HeadingMixedWithOtherText()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("Текст перед # Заголовок\nЕще текст").Should().Be("Текст перед <h1>Заголовок</h1>\nЕще текст");
        md.Render("# Заголовок\nОбычный текст\n# Второй").Should().Be("<h1>Заголовок</h1>\nОбычный текст\n<h1>Второй</h1>");
    }

    [Test]
    public void Test_NestedTagsInsideHeading()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("# _italic_ **bold** text").Should().Be("<h1><em>italic</em> **bold** text</h1>");
        md.Render("# **bold** _italic_ text").Should().Be("<h1>**bold** <em>italic</em> text</h1>");
    }
}