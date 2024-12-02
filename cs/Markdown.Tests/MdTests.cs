using FluentAssertions;
using FluentAssertions.Extensions;
using Markdown.Converter;
using Markdown.Tokenizer;
using Markdown.TokenParser;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdTests
{
    [TestCase("Test", "Test", TestName = "ParagraphWord")]
    [TestCase("# Test", "<h1>Test</h1>", TestName = "HeadingWord")]
    [TestCase("__Test", "__Test", TestName = "UnpairedStrong")]
    [TestCase("_Test", "_Test", TestName = "UnpairedEmphasis")]
    [TestCase("_Te_st", "<em>Te</em>st", TestName = "EmphasisPartOfWordAtBegin")]
    [TestCase("T_es_t", "T<em>es</em>t", TestName = "EmphasisPartOfWordAtMiddle")]
    [TestCase("Te_st_", "Te<em>st</em>", TestName = "EmphasisPartOfWordAtEnd")]
    [TestCase("__Te__st", "<strong>Te</strong>st", TestName = "StrongPartOfWordAtBegin")]
    [TestCase("T__es__t", "T<strong>es</strong>t", TestName = "StrongPartOfWordAtMiddle")]
    [TestCase("Te__st__", "Te<strong>st</strong>", TestName = "StrongPartOfWordAtEnd")]
    [TestCase("# Test # Hash", "<h1>Test # Hash</h1>", TestName = "HashInsideHeading")]
    [TestCase("_Test_", "<em>Test</em>", TestName = "ParagraphEmphasisWord")]
    [TestCase("__Test__", "<strong>Test</strong>", TestName = "ParagraphStrongWord")]
    [TestCase("# __Test__", "<h1><strong>Test</strong></h1>", TestName = "StrongWord")]
    [TestCase("_Test_ __Test__", "<em>Test</em> <strong>Test</strong>", TestName = "ParagraphEmphasisAndStrong")]
    [TestCase("# _Test_ __Test__", "<h1><em>Test</em> <strong>Test</strong></h1>", TestName = "HeadingEmphasisAndStrong")]
    [TestCase("__Strong _Emphasis_ Strong__", "<strong>Strong <em>Emphasis</em> Strong</strong>", TestName = "ParagraphEmphasisInsideStrong")]
    [TestCase("_Emphasis __Strong__ Emphasis_", "<em>Emphasis __Strong__ Emphasis</em>", TestName = "ParagraphStrongInsideEmphasis")]
    [TestCase("# __Strong _Emphasis_ Strong__", "<h1><strong>Strong <em>Emphasis</em> Strong</strong></h1>", TestName = "HeadingEmphasisInsideStrong")]
    [TestCase("Test\nTest", "Test\nTest", TestName = "TextWithSeveralParagraphs")]
    [TestCase("Test\r\nTest", "Test\r\nTest", TestName = "TextWithSeveralParagraphsWithCarriageReturn")]
    [TestCase("# Test\nTest", "<h1>Test</h1>\nTest", TestName = "HeadingAndParagraph")]
    [TestCase("- Пункт первый", "<ul><li>Пункт первый</li></ul>", TestName = "UnorderedListWithOneListItem")]
    [TestCase("- Пункт первый\n- Пункт второй\n- Пункт третий", "<ul><li>Пункт первый</li><li>Пункт второй</li><li>Пункт третий</li></ul>", TestName = "UnorderedListWithSeveralListItem")]
    [TestCase("- Пункт первый\r\n- Пункт второй\r\n- Пункт третий", "<ul><li>Пункт первый</li><li>Пункт второй</li><li>Пункт третий</li></ul>", TestName = "UnorderedListWithSeveralListItemWithCarriageReturn")]
    [TestCase("- Список первый\n+ Список второй\n* Список третий", "<ul><li>Список первый</li></ul><ul><li>Список второй</li></ul><ul><li>Список третий</li></ul>", TestName = "UnorderedListsWithDifferentBullet")]
    [TestCase("- Уровень 1.1\n    - Уровень 2.1\n- Уровень 1.2\n    - Уровень 2.1\n    - Уровень 2.2", "<ul><li>Уровень 1.1<ul><li>Уровень 2.1</li></ul></li><li>Уровень 1.2<ul><li>Уровень 2.1</li><li>Уровень 2.2</li></ul></li></ul>", TestName = "NestedUnorderedLists")]
    [TestCase("__s _E _e_ E_ s__", "<strong>s <em>E <em>e</em> E</em> s</strong>", TestName = "EmphasisInsideEmphasis")]
    [TestCase("__s __E _e_ E__ s__", "<strong>s <strong>E <em>e</em> E</strong> s</strong>", TestName = "StrongInsideStrong")]
    [TestCase("_Intersection __Emphasis And_ Strong__", "_Intersection __Emphasis And_ Strong__", TestName = "IntersectionEmphasisAndStrong")]
    [TestCase("__Intersection _Strong And__ Emphasis_", "__Intersection _Strong And__ Emphasis_", TestName = "IntersectionStrongAndEmphasis")]
    [TestCase("- # Heading", "<ul><li># Heading</li></ul>", TestName = "HeadingInUnorderedList")]
    [TestCase("- __Strong__", "<ul><li><strong>Strong</strong></li></ul>", TestName = "StrongInUnorderedList")]
    [TestCase("- _Emphasis_", "<ul><li><em>Emphasis</em></li></ul>", TestName = "EmphasisInUnorderedList")]
    [TestCase("- Nested tags\n    + _Emphasis __Strong__ Emphasis_\n    + __Strong _Emphasis_ Strong__", "<ul><li>Nested tags<ul><li><em>Emphasis __Strong__ Emphasis</em></li><li><strong>Strong <em>Emphasis</em> Strong</strong></li></ul></li></ul>", TestName = "NestedTagsInUnorderedList")]
    [TestCase("- Intersections\n    + _Intersection __Emphasis And_ Strong__\n    + __Intersection _Strong And__ Emphasis_", "<ul><li>Intersections<ul><li>_Intersection __Emphasis And_ Strong__</li><li>__Intersection _Strong And__ Emphasis_</li></ul></li></ul>", TestName = "IntersectionsInUnorderedList")]
    
    public void Render_CorrectHtml_When(string markdown, string expectedHtml)
    {
        var md = new Md(new MdTokenizer(), new MdTokenParser(), new MdToHtmlConverter());

        var renderResult = md.Render(markdown);

        renderResult.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Render_PerformanceTest()
    {
        var md = new Md(new MdTokenizer(), new MdTokenParser(), new MdToHtmlConverter());
        
        const string markdown = "# Заголовок __с _разными_ символами__";

        const int iterations = 100000;
        
        var renderMarkdown = () =>
        {
            for (var i = 0; i < iterations; i++)
            {
                md.Render(markdown);
            }
        };
        
        renderMarkdown.ExecutionTime().Should().BeLessThan(10.Seconds());
    }
}