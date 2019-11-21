using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public static readonly Dictionary<TokenType, string> DefaultStringForTokenTypes;
        public static readonly Token EmptyToken;

        static Token()
        {
            DefaultStringForTokenTypes = new Dictionary<TokenType, string>
            {
                { TokenType.Empty, ""},
                { TokenType.Underscore, "_"},
                { TokenType.DoubleUnderscores, "__"}
            };
            EmptyToken = new Token(TokenType.Empty, 0, "");
        }

        public int Position { get; }
        public string Text { get; }
        public int Length => Text.Length;
        public bool IsEmpty => Length == 0;
        public TokenType TokenType;

        public Token(TokenType tokenType, int position, string text = null)
        {
            TokenType = tokenType;
            Position = position;
            if (text == null)
            {
                if (DefaultStringForTokenTypes.ContainsKey(tokenType))
                    Text = DefaultStringForTokenTypes[tokenType];
                else
                    throw new ArgumentException("Default string for token type not found");
            }
            else
                Text = text;
        }

        public static string ConcatenateTokens(List<Token> tokens)
        {
            var builder = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token.TokenType == TokenType.EscapedSymbol)
                    builder.Append(token.Text[1]);
                else
                    builder.Append(token.Text);
            }
            return builder.ToString();
        }
    }
}