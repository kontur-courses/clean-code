using System.Text;
using Markdown.Tokens;

namespace Markdown.MdParsing
{
    public static class MdSymbols_Handler
    {
        private static readonly HashSet<char> specialSymbols = ['_', '\\', ' '];

        private static bool IsSpecSymbol(char symbol) => specialSymbols.Contains(symbol);

        private static void HandleHashSymbol(List<Token> tokens, string paragraph, int currentIndex)
        {
            if (currentIndex != 0 || 2 > paragraph.Length || paragraph[1] != ' ')
            {
                HandleText(tokens, paragraph, currentIndex);
                return;
            }

            tokens.Add(new Token(TokenType.Tag, "# "));
        }

        public static void HandleSymbol(List<Token> tokens, string paragraph, int currentIndex)
        {
            switch (paragraph[currentIndex])
            {
                case '#':
                    HandleHashSymbol(tokens, paragraph, currentIndex);
                    return;
                case '\\':
                    HandleEscapeSymbol(tokens);
                    return;
                case '_':
                    HandleUnderscore(tokens, paragraph, currentIndex);
                    return;
                case ' ':
                    HandleSpace(tokens);
                    return;
                default:
                    HandleText(tokens, paragraph, currentIndex);
                    return;
            }
        }

        private static void HandleSpace(List<Token> tokens)
        {
            tokens.Add(new Token(TokenType.Space, " "));
        }

        private static void HandleText(List<Token> tokens, string paragraph, int currentIndex)
        {
            var stringBuilder = new StringBuilder();
            var tokenType = TokenType.Space;
            for (var i = currentIndex; i < paragraph.Length; i++)
            {
                if (tokenType == TokenType.Space)
                    tokenType = char.IsNumber(paragraph[currentIndex]) ? TokenType.Number : TokenType.Text;
                if (tokenType == TokenType.Text &&
                    (char.IsNumber(paragraph[currentIndex]) || IsSpecSymbol(paragraph[currentIndex])))
                    break;
                if (tokenType == TokenType.Number && !char.IsNumber(paragraph[currentIndex]))
                    break;
                stringBuilder.Append(paragraph[currentIndex]);
                currentIndex++;
            }

            tokens.Add(new Token(tokenType, stringBuilder.ToString()));
        }

        private static void HandleUnderscore(List<Token> tokens, string paragraph, int currentPosition)
        {
            if (currentPosition + 1 < paragraph.Length && paragraph[currentPosition + 1] == '_')
                tokens.Add(new Token(TokenType.Tag, "__"));
            else
                tokens.Add(new Token(TokenType.Tag, "_"));
        }

        private static void HandleEscapeSymbol(List<Token> tokens)
        {
            tokens.Add(new Token(TokenType.Escape, "\\"));
        }
    }
}
