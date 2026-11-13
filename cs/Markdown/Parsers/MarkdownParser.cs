using System.Text;
using Markdown.Abstractions;
using Markdown.Models;
using Markdown.Parsers.MarkdownParsers;

namespace Markdown.Parsers;

public class MarkdownParser : ITextParser
{
    public MarkedText Parse(string markdownText)
    {
        return new ParsingContext(markdownText).Parse();
    }

    private sealed class ParsingContext
    {
        private readonly StringBuilder buffer;
        private readonly EmphasisParser emphasisParser;
        private readonly EscapeParser escapeParser;

        private readonly HeaderParser headerParser;
        private readonly string text;
        private readonly List<Token> tokens;

        public ParsingContext(string text)
        {
            this.text = text;
            buffer = new StringBuilder();
            tokens = [];
            headerParser = new HeaderParser(text, buffer, tokens);
            escapeParser = new EscapeParser(text, buffer, tokens);
            emphasisParser = new EmphasisParser(text, buffer, tokens);
        }

        public MarkedText Parse()
        {
            for (var i = 0; i < text.Length; i++)
            {
                var wasHeadingOpen = headerParser.IsHeadingOpen;
                var headerHandled = headerParser.TryParse(ref i);

                if (wasHeadingOpen != headerParser.IsHeadingOpen)
                    emphasisParser.ResetPendingMarkers();

                if (headerHandled)
                    continue;

                if (escapeParser.TryParse(ref i))
                    continue;

                if (emphasisParser.TryParse(ref i))
                    continue;

                buffer.Append(text[i]);
            }

            FlushText();

            if (headerParser.IsHeadingOpen)
                tokens.Add(new Token(TokenKind.HeadingEnd));

            return new MarkedText(tokens);
        }

        private void FlushText()
        {
            if (buffer.Length > 0)
                tokens.Add(new Token(TokenKind.Text, buffer.ToString()));
            buffer.Clear();
        }
    }
}