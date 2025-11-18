using FluentAssertions;
using Markdown.Parsing;
using Markdown.Tokenizing;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdTests
{
    private Md md;

    [SetUp]
    public void Setup()
    {
        md = new Md(new Lexer(), new Parser(), new HtmlRender());
    }

    [Test]
    public void Italics_RenderToEm()
    {
        var result = md.Render("_окруженный сторон_");
        result.Should().Be("<em>окруженный сторон</em>");
    }

    [Test]
    public void Strong_RenderToStrong()
    {
        var result = md.Render("__жирный__");
        result.Should().Be("<strong>жирный</strong>");
    }
    [Test]
    public void Strong_NotRenderToStrongWithSpaceClose()
    {
        var result = md.Render("__жирный __");
        result.Should().Be("__жирный __");
    }
    [Test]
    public void Strong_NotRenderToStrongWithSpaceOpen()
    {
        var result = md.Render("__ жирный__");
        result.Should().Be("__ жирный__");
    }
    [Test]
    public void Strong_NotRenderToStrongWithSpaceCloseSingle()
    {
        var result = md.Render("_жирный _");
        result.Should().Be("_жирный _");
    }
    [Test]
    public void Strong_NotRenderToStrongWithSpaceOpenSingle()
    {
        var result = md.Render("_ жирный_");
        result.Should().Be("_ жирный_");
    }

    [Test]
    public void NestedItalicsInStrong_RenderCorrectly()
    {
        var result = md.Render("__с _разными_ тегами__");
        result.Should().Be("<strong>с <em>разными</em> тегами</strong>");
    }

    [Test]
    public void NestedStrongInsideItalics_NotRender()
    {
        var result = md.Render("_внутри __одинарного двойной__ не_ работает");
        result.Should().Be("<em>внутри __одинарного двойной__ не</em> работает");
    }

    [Test]
    public void EscapedUnderscore_StayUnderscore()
    {
        var result = md.Render(@"\_вот это\_");
        result.Should().Be("_вот это_");
    }

    [Test]
    public void DoubleEscape_Work()
    {
        var result = md.Render(@"\\_вот это будет курсивом_");
        result.Should().Be("\\<em>вот это будет курсивом</em>");
    }

    [Test]
    public void Heading_RenderToH1()
    {
        var result = md.Render("# Заголовок");
        result.Should().Be("<h1>Заголовок</h1>");
    }

    [Test]
    public void HeadingWithFormatting_RenderCorrectly()
    {
        var result = md.Render("# Заголовок __с _разными_ символами__");
        result.Should().Be("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>");
    }

    [Test]
    public void UnderscoreInsideNumbers_NotRender()
    {
        var result = md.Render("a_12_3");
        result.Should().Be("a_12_3");
    }
    
    [Test]
    public void DoubleUnderscoreInsideNumbers_NotRender()
    {
        var result = md.Render("a__12__3");
        result.Should().Be("a__12__3");
    }
    
    [Test]
    public void UnderscoreInsideWords_Render()
    {
        var result = md.Render("a_b_c");
        result.Should().Be("a<em>b</em>c");
    }
    [Test]
    public void EmptyDoubleUnderscores_NotRender()
    {
        var result = md.Render("ффф ____ ффф");
        result.Should().Be("ффф ____ ффф");
    }

    [Test]
    public void UnderscoreBetweenWords_NotRender()
    {
        var result = md.Render("ра_зных сл_овах");
        result.Should().Be("ра_зных сл_овах");
    }
    
    [Test]
    public void DoubleUnderscoreBetweenWords_NotRender()
    {
        var result = md.Render("ра__зных сл__овах");
        result.Should().Be("ра__зных сл__овах");
    }
    
    [Test]
    public void DifferentUnderscoreWords_NotRender()
    {
        var result = md.Render("__Непарные_ символы");
        result.Should().Be("__Непарные_ символы");
    }
    
    [Test]
    public void UnderscoreBeforeSpace_NotRender()
    {
        var result = md.Render("_ не курсив");
        result.Should().Be("_ не курсив");
    }

    [Test]
    public void UnderscoreAfterSpace_NotRender()
    {
        var result = md.Render("не _ курсив");
        result.Should().Be("не _ курсив");
    }

    [Test]
    public void IntersectingTags_RenderAsText()
    {
        var result = md.Render("__пересечение _двойных__ и одинарных_");
        result.Should().Be("__пересечение _двойных__ и одинарных_");
    }
    [Test]
    public void IntersectingUnderscoreTags_RenderAsText()
    {
        var result = md.Render("_пересечение __двойных_ и одинарных__");
        result.Should().Be("_пересечение __двойных_ и одинарных__");
    }
    
    [Test] 
    public void IntersectingTags_RenderAsTexts()
    {
        var result = md.Render("__пересечение _двойных__ и одинарных");
        result.Should().Be("<strong>пересечение _двойных</strong> и одинарных");
    }
    
    [Test]
    public void IntersectingUnderscoreTags_RenderAsTexts()
    {
        var result = md.Render("_пересечение __двойных_ и одинарных");
        result.Should().Be("<em>пересечение __двойных</em> и одинарных");
    }

    [Test]
    public void EmptyUnderscoreInner_NotRenderItalics()
    {
        var result = md.Render("aa __ __ bb");
        result.Should().Be("aa __ __ bb");
    }

    [Test]
    public void StrongWithoutClosing_RenderUnderscores()
    {
        var result = md.Render("__нет конца");
        result.Should().Be("__нет конца");
    }

    [Test]
    public void StrongWithoutOpening_RenderUnderscores()
    {
        var result = md.Render("нет начала__");
        result.Should().Be("нет начала__");
    }

    [Test]
    public void MultipleParagraphs_RenderSeparate()
    {
        var result = md.Render("первый абзацвторой абзац");
        result.Should().Be("первый абзацвторой абзац");
    }

    [Test]
    public void HeadingFollowedByParagraph_RenderBoth()
    {
        var result = md.Render("# Заголовоктекст");
        result.Should().Be("<h1>Заголовоктекст</h1>");
    }
    

    [Test]
    public void StrongAtEndOfText_RenderCorrect()
    {
        var result = md.Render("тест __bold__");
        result.Should().Be("тест <strong>bold</strong>");
    }

    [Test]
    public void FullDocument_RenderLargeMarkdownSample()
    {
        var markdown = "# Глава 1\n" +
                       "Текст _курсив_ и __жирный__.\n\n" +
                       "# Раздел 1.1\n" +
                       "_Пересечение __двойных_ и одинарных__ должно остаться текстом.";

        var result = md.Render(markdown);

        result.Should().Be("<h1>Глава 1</h1>" +
                           "Текст <em>курсив</em> и <strong>жирный</strong>." + 
                           "<h1>Раздел 1.1</h1>" +
                           "_Пересечение __двойных_ и одинарных__ должно остаться текстом.");
    }

    [Test]
    public void InlineLink_RenderAnchor()
    {
        var result = md.Render("Читайте [Skillbox Media](https://skillbox.ru/media/)");
        result.Should().Be("Читайте <a href=\"https://skillbox.ru/media/\">Skillbox Media</a>");
    }

    [Test]
    public void AutoLink_RenderAnchorWithUrlAsText()
    {
        var result = md.Render("Ссылка: <https://skillbox.ru/media/code/>");
        result.Should().Be("Ссылка: <a href=\"https://skillbox.ru/media/code/\">https://skillbox.ru/media/code/</a>");
    }

    [Test]
    public void InvalidAutoLink_StayText()
    {
        var result = md.Render("Ссылка: <ftp://skillbox.ru/media/>");
        result.Should().Be("Ссылка: <ftp://skillbox.ru/media/>");
    }

    [Test]
    public void Parser_ProduceLinkNode()
    {
        var lexer = new Lexer();
        
        var tokens = lexer.Tokenize("[Skillbox Media](https://skillbox.ru/media/)");
        var parser = new Parser();
        var document = parser.Parse(tokens);
        document.Children.Should().ContainSingle()
            .Which.Should().BeOfType<LinkNode>()
            .Which.Href.Should().Be("https://skillbox.ru/media/");
    }

    [Test]
    public void LinkParser_RecognizeBracketLink()
    {
        var lexer = new Lexer();
        var tokens = lexer.Tokenize("[text](https://example.com)");
        var cursor = new TokenIndexer(tokens);
        LinkParser.CanStartLink(cursor).Should().BeTrue();

        var nodes = new List<INode>();
        LinkParser.TryParseLink(cursor, nodes).Should().BeTrue();
        nodes.Should().ContainSingle().Which.Should().BeOfType<LinkNode>();
    }
}