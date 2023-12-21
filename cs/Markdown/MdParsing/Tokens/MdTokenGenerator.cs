using System.Text;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.MdParsing.Tokens
{
    public static class MdTokenGenerator
    {
        private static readonly HashSet<char> specialSymbols = ['_', '\\', ' '];
        private static bool IsSpecSymbol(char symbol) => specialSymbols.Contains(symbol);
        
        public static Token GetTokenBySymbol(string paragraph, int currentIndex) =>
            paragraph[currentIndex] switch
            {
                '#' => GetHashToken(paragraph, currentIndex),
                '\\' => GetEscapeToken(),
                '_' => GetUnderscoreToken(paragraph, currentIndex),
                ' ' => GetSpaceToken(),
                _ => GetTextToken(paragraph, currentIndex)
            };

        private static Token GetHashToken(string paragraph, int currentIndex) =>
            currentIndex != 0 || 2 > paragraph.Length || paragraph[1] != ' '
                ? GetTextToken(paragraph, currentIndex)
                : new Token(TokenType.Tag, "# ", TagType.Header);
        
        private static Token GetSpaceToken() => 
            new(TokenType.Space, " ");

        private static Token GetTextToken(string paragraph, int currentIndex)
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

            return new Token(tokenType, stringBuilder.ToString());
        }

        private static Token GetUnderscoreToken(string paragraph, int currentPosition) =>
            currentPosition + 1 < paragraph.Length && paragraph[currentPosition + 1] == '_'
                ? new Token(TokenType.Tag, "__", TagType.Bold)
                : new Token(TokenType.Tag, "_", TagType.Italic);

        private static Token GetEscapeToken() => 
            new(TokenType.Escape, "\\", TagType.Escape);
    }
}
