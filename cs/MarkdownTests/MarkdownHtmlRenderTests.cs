﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.Parsing;
using Markdown.Parsing.Tags;
using Markdown.Render;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownHtmlRenderTests
{
    private Md? _md;

    [SetUp]
    public void SetUp()
    {
        var italicTag = new ItalicMdTag();
        var boldTag = new BoldMdTag();

        var mdParser = new MarkdownParser(new List<IMdTag>()
        {
            italicTag,
            boldTag
        });

        _md = new Md(mdParser!.Tags.ToList(), new HtmlMdRenderer(DefaultHtmlReplaceRules.CreateDefaultRules()));
    }


    [TestCase("_testfs gdfgsd fgвфыв df_", "<em>testfs gdfgsd fgвфыв df</em>", TestName = "Последовательность слов, разделенных пробелами")]
    [TestCase("_testfgdf_uljkl", "<em>testfgdf</em>", TestName = "Тег в начале и в середине слова")]
    [TestCase("testfgdf_uljkl_", "<em>uljkl</em>", TestName = "Тег в середине и в конце слова")]
    [TestCase("test_fgdf_uljkl", "<em>fgdf</em>", TestName = "Тег в середине слова")]
    public void Parse_ItalicTag_RightMatch(string sourceText, string matchText)
    {
        var resultRender = _md!.Render(sourceText);
        resultRender.Should().Contain(matchText);
    }

    [TestCase("__testfs gdfgsd fgвфыв df__", "<strong>testfs gdfgsd fgвфыв df</strong>", TestName = "Последовательность слов, разделенных пробелами")]
    public void Parse_BoldTag_RightMatch(string sourceText, string matchText)
    {
        var resultRender = _md!.Render(sourceText);
        resultRender.Should().Contain(matchText);
    }
    
    [Test]
    public void Parse_ItalicTagInsideBold_NotRightMatch()
    {
        var resultRender = _md!.Render("___asdasd___");
        resultRender.Should().Contain("<strong><em>asdasd</em></strong>");
    }
}