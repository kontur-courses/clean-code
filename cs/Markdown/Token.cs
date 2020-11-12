using System.Collections.Generic;
using System.Threading;

namespace Markdown
{
    public class Token
    {
        public readonly int StartIndex;
        public readonly int Length;
        public readonly string Value;
        public readonly TokenType Type;
        public readonly List<Token> InsertedTokens;

        public Token(int length, int startIndex, TokenType type, string value)
        {
            this.StartIndex = startIndex;
            this.Length = length;
            this.Type = type;
            this.Value = value;
            InsertedTokens = new List<Token>();
        }

        public static Token CreateBoldToken(int startIndex, int endIndex, string line, List<Token> inserted)
        {
            var lengthWithoutBorders = endIndex - startIndex - 3;
            var value  = line.Substring(startIndex + 2, lengthWithoutBorders);
            var token = new Token(lengthWithoutBorders + 4, startIndex, TokenType.Bold, value);
            foreach (var insertedToken in inserted)
            {
                token.AddInsertedToken(insertedToken);
            }

            return token;
        }

        public void AddInsertedToken(Token inserted)
        {
            InsertedTokens.Add(inserted);
        }

        public static Token Empty()
        {
            return new Token(0, 0, TokenType.Bold, "");
        }
    }
    
    public enum TokenType{
        Italic,
        Bold,
        Header
    }
}