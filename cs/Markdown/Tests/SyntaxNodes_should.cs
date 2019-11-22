using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown.Languages;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class SyntaxNodes_should
    {
        private static string value;

        [SetUp]
        public void SetUp()
        {
            value = "aba aba";
        }

        [TestCase("Html")]
        [TestCase("MarkDown")]
        public void TextNodeShould_ConvertTo_WhenAnyLanguage_ReturnValue(string typeName)
        {
            var textNode = new TextNode(value);
            var language = LanguageRegistry.BuildLanguage(typeName);
            textNode.BuildLinesWithTag(language.Tags)
                .Should().Be(value);
        }

        [TestCase("Html", TagType.Em)]
        [TestCase("Html", TagType.Strong)]
        [TestCase("MarkDown", TagType.Em)]
        [TestCase("MarkDown", TagType.Strong)]
        public void TagNodeShould_ConvertTo_WhenAnyLanguageAndTextNode_AddInStartAndEndTag(string typeName,
            TagType tagType)
        {
            var tagNode = new TagNode(tagType, new List<SyntaxNode>() {new TextNode(value)});
            var language = LanguageRegistry.BuildLanguage(typeName);
            var result = tagNode.BuildLinesWithTag(language.Tags);
            result.Should().StartWith(language.Tags[tagType].Start);
            result.Should().EndWith(language.Tags[tagType].End);
        }

        [TestCase("Html")]
        [TestCase("MarkDown")]
        public void SyntaxTreeShould_ConvertTo_WhenAnyLanguageAndTwoTextNode_JoinText(string typeName)
        {
            var textNode = new TextNode(value);
            var syntaxTree = new SyntaxTree(new List<SyntaxNode>() {textNode, textNode});
            var language = LanguageRegistry.BuildLanguage(typeName);
            var result = syntaxTree.BuildLinesWithTag(language.Tags);
            result.Should().Be(value + value);
        }

        [TestCase("Html", TagType.Em)]
        [TestCase("Html", TagType.Strong)]
        [TestCase("MarkDown", TagType.Em)]
        [TestCase("MarkDown", TagType.Strong)]
        public void SyntaxTreeShould_ConvertTo_WhenAnyLanguageAndTextNodeAndTagNode_JoinText(string typeName,
            TagType tagType)
        {
            var textNode = new TextNode(value);
            var tagNode = new TagNode(tagType, new List<SyntaxNode>() {textNode});
            var syntaxTree = new SyntaxTree(new List<SyntaxNode>() {tagNode, textNode});
            var language = LanguageRegistry.BuildLanguage(typeName);
            var result = syntaxTree.BuildLinesWithTag(language.Tags);
            result.Should().Be(language.Tags[tagType].Start + value + language.Tags[tagType].End + value);
        }

        [TestCase("Html", TagType.Em, null)]
        [TestCase("Html", TagType.Strong, null)]
        [TestCase("MarkDown", TagType.Em, null)]
        [TestCase("MarkDown", TagType.Strong, null)]
        [TestCase("Html", TagType.Em, "")]
        [TestCase("Html", TagType.Strong, "")]
        [TestCase("MarkDown", TagType.Em, "")]
        [TestCase("MarkDown", TagType.Strong, "")]
        public void TagNodeShould_ConvertTo_WhenAnyLanguageAndTagNodeWithNullOrEmpty_ReturnEmptyWithTag(string typeName,
            TagType tagType, string str)
        {
            var textNode = new TextNode(null);
            var tagNode = new TagNode(tagType, new List<SyntaxNode>() {textNode});
            var language = LanguageRegistry.BuildLanguage(typeName);
            var result = tagNode.BuildLinesWithTag(language.Tags);
            result.Should().Be(language.Tags[tagType].Start + language.Tags[tagType].End);
        }
    }
}