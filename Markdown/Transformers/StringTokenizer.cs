using Markdown.Tokens;

namespace Markdown.Transformers;

public static class StringTokenizer
{
    public static IReadOnlyList<Token> Tokenize(string text)
        => new Scanner(text).Run();

    private sealed class Scanner(string text)
    {
        private readonly List<Token> _tokens = [];

        public IReadOnlyList<Token> Run()
        {
            var paragraphs = text.Split("\n");

            foreach (var line in paragraphs)
            {
                TokenizeLine(line);
            }


            return _tokens;
        }


        private void TokenizeLine(string line)
        {
            var cursor = 0;

            if (line.StartsWith(Token.HeaderPrefix))
            {
                _tokens.Add(Token.HeaderMarker());
                cursor = Token.HeaderPrefix.Length;
            }

            var pendingTextStart = cursor;

            while (cursor < line.Length)
            {
                var delimiter = FindDelimeter(line, cursor);
                if (delimiter is not var (pattern, createToken))
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

        private static (bool CanOpen, bool CanClose) GetFlagsForDelimiter(string line, int cursor, string pattern)
        {
            var indexAfterDelimiter = cursor + pattern.Length;

            var characterBeforeCursor = cursor > 0 ? line[cursor - 1] : ' ';
            var characterAfterDelimiter =
                indexAfterDelimiter < line.Length ? line[indexAfterDelimiter] : ' ';

            var isDelimiterBetweenDigits = char.IsDigit(characterAfterDelimiter) || char.IsDigit(characterBeforeCursor);


            return (CanOpen: !char.IsWhiteSpace(characterAfterDelimiter) && !isDelimiterBetweenDigits,
                CanClose: !char.IsWhiteSpace(characterBeforeCursor) && !isDelimiterBetweenDigits);
        }


        private void FlushPendingText(string line, int from, int to)
        {
            if (from < to)
                _tokens.Add(Token.Text(line[from..to]));
        }


        private static (string Pattern, Func<bool, bool, Token> Create)? FindDelimeter(string line, int cursor)
        {
            foreach (var delimiter in Token.Delimeters)
            {
                if (string.CompareOrdinal(line, cursor, delimiter.Pattern, 0, delimiter.Pattern.Length) == 0)
                {
                    return delimiter;
                }
            }

            return null;
        }
    }
}