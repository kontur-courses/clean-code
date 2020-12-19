using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token : IToken
    {
        public int StartPosition { get; set; }
        public int EndPosition => StartPosition + Length;
        public int Length { get; set; }
        public TokenType Type { get; set; }
        public List<IToken> SubTokens { get; set; }
        public IToken Parent { get; set; }
        public Tag OpeningTag { get; set; }
        public Tag ClosingTag { get; set; }
        public bool ContainsOnlyDigits { get; set; }

        public Token(int startPos, int length, IToken parent, TokenType type)
        {
            Type = type;
            OpeningTag = Tag.GetTagByTokenType(type, true);
            ClosingTag = Tag.GetTagByTokenType(type, false);
            StartPosition = startPos;
            Length = length;
            SubTokens = new List<IToken>();
            Parent = parent;
        }

        public void Open(Stack<IToken> openedTokens)
        {
            Parent = openedTokens.Peek();
            openedTokens.Peek().SubTokens.Add(this);
            openedTokens.Push(this);
        }

        public void Close(int position)
        {
            Length = position - StartPosition + 1;
        }
    }
}