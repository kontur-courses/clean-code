namespace Markdown.MDParsing
{
    public static class InlineTokenValidator
    {
        public static bool CanOpen(IReadOnlyList<Token> tokens, int index) =>
            index + 1 < tokens.Count && !StartsWithWhitespace(tokens[index + 1]);

        public static bool CanClose(IReadOnlyList<Token> tokens, int index) =>
            index > 0 && !EndsWithWhitespace(tokens[index - 1]);

        public static bool HasTextBefore(IReadOnlyList<Token> tokens, int index) =>
            index > 0 && IsText(tokens[index - 1]) && char.IsLetterOrDigit(tokens[index - 1].Text[^1]);

        public static bool HasTextAfter(IReadOnlyList<Token> tokens, int index) =>
            index + 1 < tokens.Count && IsText(tokens[index + 1]) && char.IsLetterOrDigit(tokens[index + 1].Text[0]);

        public static bool IsDigitBefore(IReadOnlyList<Token> tokens, int index) =>
            index > 0 && IsText(tokens[index - 1]) && char.IsDigit(tokens[index - 1].Text[^1]);

        public static bool IsDigitAfter(IReadOnlyList<Token> tokens, int index) =>
            index + 1 < tokens.Count && IsText(tokens[index + 1]) && char.IsDigit(tokens[index + 1].Text[0]);

        private static bool IsText(Token token) =>
            !token.IsUnderscores && token.Text.Length > 0;

        private static bool StartsWithWhitespace(Token token) =>
            IsText(token) && char.IsWhiteSpace(token.Text[0]);

        private static bool EndsWithWhitespace(Token token) =>
            IsText(token) && char.IsWhiteSpace(token.Text[^1]);
    }
}