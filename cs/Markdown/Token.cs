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

        public Token(int startIndex, int endIndex, string line, TokenType type, List<Token> inserted)
        {
            var lengthWithoutBorders = endIndex - startIndex - 3;
            this.Length = lengthWithoutBorders + 4;
            this.StartIndex = startIndex;
            this.Type = type;
            this.Value = line.Substring(startIndex + 2, lengthWithoutBorders);
            this.InsertedTokens = inserted;
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