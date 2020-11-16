using System.Collections.Generic;
using FluentAssertions;
using Markdown.Converter;
using Markdown.Markup;
using Markdown.Tags;
using NUnit.Framework;

namespace MarkdownTests.ConverterTests
{
    public class ConverterTests
    {
        private readonly Dictionary<string, Tag> supportedTags =
            new Dictionary<string, Tag>
            {
                {"_", new Tag("_", "em", "_")},
                {"__", new Tag("__", "strong", "__")},
                {"#", new Tag("#", "h1", "\n")}
            };
        
        [Test]
        public void ConvertToHtmlString_ReturnHtmlString()
        {
            var expected =
                "Обычный текст<em>курсивный текст</em><strong>полужирный текст</strong><h1>заголовок</h1>";
            
            var converted = MarkupConverter.ConvertToHtmlString(new[]
            {
                new Markup("Обычный текст", null),
                new Markup("курсивный текст", supportedTags["_"]),
                new Markup("полужирный текст", supportedTags["__"]),
                new Markup("заголовок", supportedTags["#"])
            });

            converted.Should().Be(expected);
        }
    }
}