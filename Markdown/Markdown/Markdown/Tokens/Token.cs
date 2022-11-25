using System.Security.Cryptography;
using Markdown.Markdown;

namespace Markdown.Tokens
{
    public class Token : IToken
    {
        private static readonly List<char?> BlackListForClose = new() { ' ', null };
        private static readonly HashSet<char?> BlackListForOpen = new() { ' ', null, '\\', '\n', '\r' };
        public TokenType Type;
        public TokenElement Element;

        public int Position { get; set; }
        public int Length { get; set; }
        public int End => Position + Length - 1;

        public string DescriptionForImage;
        public string PathForImage;

        public Token(int position, int length)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException($"position{position} must be positive");
            Position = position;
            Length = length;
        }

        public Token(int position, int length, TokenType type)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException($"position {position} must be positive");
            Position = position;
            Length = length;
            Type = type;
        }

        public Token(int position, string description, string path)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException($"position {position} must be positive");
            Position = position;
            DescriptionForImage = description;
            PathForImage = path;
            Type = TokenType.Image;
        }

        public void SetToDefault()
        {
            Type = TokenType.Default;
        }

        public void SetToUnsee()
        {
            Type = TokenType.Unseen;
        }

        public TokenElement GetElementInText(string mdString)
        {
            char? charBeforeTag = null;
            char? charAfterTag = null;
            if (Position >= 1)
                charBeforeTag = mdString[Position - 1];
            if (Position + Length < mdString.Length)
                charAfterTag = mdString[Position + Length];
            if (BlackListForOpen.Contains(charAfterTag))
            {
                return BlackListForClose.Contains(charBeforeTag)
                    ? TokenElement.Default
                    : TokenElement.Close;
            }

            return BlackListForClose.Contains(charBeforeTag)
                ? TokenElement.Open
                : TokenElement.Unknown;
        }

        public ReadOnlySpan<char> CreateString(string md)
        {
            return md.AsSpan(Position, Length);
        }

        
    }
}
