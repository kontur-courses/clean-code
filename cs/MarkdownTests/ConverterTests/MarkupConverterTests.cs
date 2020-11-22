using System.Collections.Generic;
using FluentAssertions;
using Markdown.Converter;
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
                {"# ", new Tag("# ", "h1", "\n")}
            };
        
        [TestCase(null, "Обычный текст", "Обычный текст")]
        [TestCase("_", "Курсивный текст", "<em>Курсивный текст</em>")]
        [TestCase("__", "Полужирный текст", "<strong>Полужирный текст</strong>")]
        [TestCase("# ", "Заголовок", "<h1>Заголовок</h1>")]
        public void ConvertToHtmlString_ReturnPlainText_WhenTagIsNull(string tag, string text, string expectedMarkup)
        {
            var markup = MarkupConverter
                .ConvertTagToHtmlString(tag != null ? supportedTags[tag] : null, text);

            markup.Should().Be(expectedMarkup);
        }
    }
}