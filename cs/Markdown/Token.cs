using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public int StartPosition { get; }
        public int EndPosition => StartPosition + Length;
        public int Length { get; private set; }
        public TokenType Type { get; }
        public List<Token> SubTokens { get; }
        public HashSet<int> EscapedCharsPos { get; }
        public Token Parent { get; private set; }
        public Tag OpeningTag { get; }
        public Tag ClosingTag { get; }

        public Token(int startPos, int length, Token parent, TokenType type)
        {
            Type = type;
            OpeningTag = Tag.GetTagByTokenType(type, true);
            ClosingTag = Tag.GetTagByTokenType(type, false);
            StartPosition = startPos;
            Length = length;
            SubTokens = new List<Token>();
            Parent = parent;
            EscapedCharsPos = new HashSet<int>();
        }

        public void Open(Stack<Token> openedTokens)
        {
            Parent = openedTokens.Peek();
            openedTokens.Peek().SubTokens.Add(this);
            openedTokens.Push(this);
        }

        public void Close(int position)
        {
            Length = position + ClosingTag.MdTag.Length - StartPosition;
        }
    }
}