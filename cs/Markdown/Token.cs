using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public int StartPosition { get; set; }
        public int EndPosition => StartPosition + Length;
        public int Length { get; set; }
        public TokenType Type { get; set; }
        public List<Token> SubTokens { get; set; }
        public Token Parent { get; set; }
        public Tag OpeningTag { get; set; }
        public Tag ClosingTag { get; set; }
        public bool ContainsOnlyDigits { get; set; }

        public Token(int startPos, int length, Token parent, TokenType type)
        {
            Type = type;
            OpeningTag = Tag.GetTagByTokenType(type, true);
            ClosingTag = Tag.GetTagByTokenType(type, false);
            StartPosition = startPos;
            Length = length;
            SubTokens = new List<Token>();
            Parent = parent;
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