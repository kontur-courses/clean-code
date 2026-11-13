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
        private readonly string text;
        private readonly StringBuilder textStorage;
        private readonly List<Token> tokens;

        private readonly HeaderParser headerParser;
        private readonly EscapeParser escapeParser;
        private readonly StrongParser strongParser;
        private readonly EmphasisParser emphasisParser;

        public ParsingContext(string text)
        {
            this.text = text;
            textStorage = new StringBuilder();
            tokens = [];
            headerParser = new HeaderParser(text, textStorage, tokens);
            escapeParser = new EscapeParser(text, textStorage, tokens);
            strongParser = new StrongParser(text, textStorage, tokens);
            emphasisParser = new EmphasisParser(text, textStorage, tokens);
        }

        public MarkedText Parse()
        {
            for (var i = 0; i < text.Length; i++)
            {
                if (headerParser.TryParse(ref i))
                    continue;

                if (escapeParser.TryParse(ref i))
                    continue;

                if (strongParser.TryParse(ref i, ref emphasisParser.EmphasisOpeningTokenIndex, ref emphasisParser.IsEmphasisInvalidatedByIntersection))
                    continue;

                if (emphasisParser.TryParse(ref i))
                    continue;

                textStorage.Append(text[i]);
            }

            headerParser.FlushText();

            if (headerParser.IsHeadingOpen)
                tokens.Add(new Token(TokenKind.HeadingEnd));

            return new MarkedText(tokens);
        }
    }
}