using System.Collections.Generic;

namespace Markdown.TokenizerClasses.Scanners
{
    public class TagScanner : IScanner
    {
        private static readonly Dictionary<string, Token> Tokens = new Dictionary<string, Token>
        {
            {"_", new Token(TokenType.Underscore, "_")},
            {"\\", new Token(TokenType.EscapeChar, "\\")},
            {"\r", new Token(TokenType.CarriageReturn, "\r")},
            {"\n", new Token(TokenType.Newline, "\n")},
            {" ", new Token(TokenType.Space, " ")},
            {"0", new Token(TokenType.Digit, "0")},
            {"1", new Token(TokenType.Digit, "1")},
            {"2", new Token(TokenType.Digit, "2")},
            {"3", new Token(TokenType.Digit, "3")},
            {"4", new Token(TokenType.Digit, "4")},
            {"5", new Token(TokenType.Digit, "5")},
            {"6", new Token(TokenType.Digit, "6")},
            {"7", new Token(TokenType.Digit, "7")},
            {"8", new Token(TokenType.Digit, "8")},
            {"9", new Token(TokenType.Digit, "9")},
        };

        public bool TryScan(string text, out Token token)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var value = text[0].ToString();
                if (Tokens.ContainsKey(value))
                {
                    token = Tokens[value];
                    return true;
                }
            }

            token = Token.Null;
            return false;
        }
    }
}