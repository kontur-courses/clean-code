using System.Linq;
using FluentAssertions;
using Markdown.Languages;
using Markdown.Tokenizing;
using Markdown.Translating;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class TranslatorShould
    {
        [Test]
        public void TranslateToMarkdown()
        {
            var translator = new Translator(new MarkdownLanguage());
            var tokens = new[]
            {
                new Token(Tag.Raw, false, "Word "),
                new Token(Tag.Emphasize, true),
                new Token(Tag.Raw, false, "another"),
                new Token(Tag.Emphasize, false),
                new Token(Tag.Raw, false, " pretty "),
                new Token(Tag.Strong, true),
                new Token(Tag.Raw, false, "word"),
                new Token(Tag.Strong, false),
                new Token(Tag.Raw, false, " !"),
            };

            translator.Translate(tokens.ToList()).Should().Be("Word _another_ pretty __word__ !");
        }

        [Test]
        public void TranslateToHtml()
        {
            var translator = new Translator(new HtmlLanguage());
            var tokens = new[]
            {
                new Token(Tag.Raw, false, "Word "),
                new Token(Tag.Emphasize, true),
                new Token(Tag.Raw, false, "another"),
                new Token(Tag.Emphasize, false),
                new Token(Tag.Raw, false, " pretty "),
                new Token(Tag.Strong, true),
                new Token(Tag.Raw, false, "word"),
                new Token(Tag.Strong, false),
                new Token(Tag.Raw, false, " !"),
            };

            translator.Translate(tokens.ToList()).Should().Be("Word <em>another</em> pretty <strong>word</strong> !");
        }
    }
}
