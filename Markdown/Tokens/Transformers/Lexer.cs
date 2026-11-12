using Markdown.Tokens.Lexing;

namespace Markdown.Tokens.Transformers;

public static class Lexer
{
    public static IReadOnlyList<RawSegment> Lexize(string text) => new Scanner(text).Run();

    private sealed class Scanner(string text)
    {
        private readonly List<RawSegment> _segments = [];

        public IReadOnlyList<RawSegment> Run()
        {
            var lines = text.Split(MarkdownSyntax.ParagraphBreaker);

            for (var i = 0; i < lines.Length; i++)
            {
                if (i > 0)
                    _segments.Add(new LineBreakSegment());

                LexizeLine(lines[i]);
            }

            return _segments;
        }

        private void LexizeLine(string line)
        {
            var cursor = 0;
            var pendingTextStart = 0;

            while (cursor < line.Length)
            {
                if (IsEscapingChar(line, cursor))
                {
                    FlushPendingText(line, pendingTextStart, cursor);
                    pendingTextStart = cursor + 1;
                    cursor += 2;
                    continue;
                }

                if (!TryMatchSymbol(line, cursor, out var symbol))
                {
                    cursor++;
                    continue;
                }

                FlushPendingText(line, pendingTextStart, cursor);
                _segments.Add(new SymbolSegment(symbol.Pattern));

                cursor += symbol.Pattern.Length;
                pendingTextStart = cursor;
            }

            FlushPendingText(line, pendingTextStart, cursor);
        }

        private static bool IsEscapingChar(string line, int cursor)
            => line[cursor] == '\\'
               && cursor + 1 < line.Length
               && MarkdownSyntax.IsEscapable(line[cursor + 1]);

        private static bool TryMatchSymbol(string line, int cursor, out SymbolDefinition symbol)
        {
            foreach (var candidate in MarkdownSyntax.Symbols)
            {
                if (candidate.LineStartOnly && cursor != 0)
                    continue;

                if (string.CompareOrdinal(line, cursor, candidate.Pattern, 0, candidate.Pattern.Length) != 0)
                    continue;

                symbol = candidate;
                return true;
            }

            symbol = null!;
            return false;
        }

        private void FlushPendingText(string line, int from, int to)
        {
            if (from < to)
                _segments.Add(new TextSegment(line[from..to]));
        }
    }
}