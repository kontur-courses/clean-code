using Markdown.Tokens;

namespace Markdown.Transformers;

public static class StringTokenizer
{
    public static IReadOnlyList<Token> Tokenize(string text)
        => new Scanner(text).Run();

    private sealed class Scanner(string text)
    {
        private readonly List<Token> _tokens = [];

        private static readonly HashSet<char> Escapables =
            [.. Token.Delimiters.Select(delimiter => delimiter.Pattern[0]), Token.HeaderPrefix[0]];

        public IReadOnlyList<Token> Run()
        {
            var lines = text.Split("\n");


            for (var i = 0; i < lines.Length; i++)
            {
                if (i > 0)
                    _tokens.Add(Token.ParagraphBreak());

                TokenizeLine(lines[i]);
            }


            return _tokens;
        }


        private void TokenizeLine(string line)
        {
            var cursor = 0;

            if (line.StartsWith(Token.HeaderPrefix, StringComparison.Ordinal))
            {
                _tokens.Add(Token.HeaderMarker());
                cursor = Token.HeaderPrefix.Length;
            }

            var pendingTextStart = cursor;

            while (cursor < line.Length)
            {
                if (line[cursor] == '\\' && cursor + 1 < line.Length && IsEscapable(line[cursor + 1]))
                {
                    FlushPendingText(line, pendingTextStart, cursor);
                    pendingTextStart = cursor + 1;
                    cursor += 2;
                    continue;
                }


                if (!TryFindDelimiter(line, cursor, out var pattern, out var createToken))
                {
                    cursor++;
                    continue;
                }


                var (canOpen, canClose) = GetFlagsForDelimiter(line, cursor, pattern);

                if (!canOpen && !canClose)
                {
                    cursor += pattern.Length;
                    continue;
                }

                FlushPendingText(line, pendingTextStart, cursor);
                _tokens.Add(createToken(canOpen, canClose));


                cursor += pattern.Length;
                pendingTextStart = cursor;
            }

            FlushPendingText(line, pendingTextStart, cursor);
        }

        private static bool IsEscapable(char character) => character == '\\' || Escapables.Contains(character);

        private static (bool CanOpen, bool CanClose) GetFlagsForDelimiter(string line, int cursor, string pattern)
        {
            var indexAfterDelimiter = cursor + pattern.Length;

            var characterBeforeCursor = cursor > 0 ? line[cursor - 1] : ' ';
            var characterAfterDelimiter =
                indexAfterDelimiter < line.Length ? line[indexAfterDelimiter] : ' ';

            var isDelimiterNearDigits = char.IsDigit(characterAfterDelimiter) || char.IsDigit(characterBeforeCursor);


            return (CanOpen: !char.IsWhiteSpace(characterAfterDelimiter) && !isDelimiterNearDigits,
                CanClose: !char.IsWhiteSpace(characterBeforeCursor) && !isDelimiterNearDigits);
        }


        private void FlushPendingText(string line, int from, int to)
        {
            if (from < to)
                _tokens.Add(Token.Text(line[from..to]));
        }


        private static bool TryFindDelimiter(
            string line,
            int cursor,
            out string pattern,
            out Func<bool, bool, Token> createToken)
        {
            foreach (var delimiter in Token.Delimiters)
            {
                if (string.CompareOrdinal(line, cursor, delimiter.Pattern, 0, delimiter.Pattern.Length) != 0) continue;
                pattern = delimiter.Pattern;
                createToken = delimiter.Create;
                return true;
            }

            pattern = null!;
            createToken = null!;
            return false;
        }
    }
}