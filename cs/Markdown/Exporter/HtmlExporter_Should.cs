using FluentAssertions;
using Markdown.Lexer;
using Markdown.Parser;
using NUnit.Framework;

namespace Markdown.Exporter
{
    [TestFixture]
    public class HtmlExporter_Should
    {
        [Test]
        public void Export_ReturnCorrectHtml_OnText()
        {
            var text = new Text("foo");

            var html = text.Export(new HtmlExporter());

            html.Should().Be(text.Value);
        }
        
        [Test]
        public void Export_ReturnCorrectHtml_OnItalicElement()
        {
            const string text = "bar";
            var italic = new MarkdownItalicElement(LexemeDefinitions.Italic.Representation);
            italic.ChildNodes.Add(new Text(text));

            var html = italic.Export(new HtmlExporter());

            html.Should().Be($"<em>{text}</em>");
        }
        
        [Test]
        public void Export_ReturnCorrectHtml_OnBoldElement()
        {
            const string text = "baz";
            var italic = new MarkdownBoldElement(LexemeDefinitions.Bold.Representation);
            italic.ChildNodes.Add(new Text(text));

            var html = italic.Export(new HtmlExporter());

            html.Should().Be($"<strong>{text}</strong>");
        }
    }
}