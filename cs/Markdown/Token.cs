using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Token
    {
        public int StartPosition { get; set; }
        public int Length { get; set; }
        public List<Token> SubTokens { get; set; }
        public TokenType Type { get; set; }
        public Token Parent { get; set; }
        public int EndPosition => StartPosition + Length;
        public string OpeningTag { get; set; }
        public string ClosingTag { get; set; }
        public string HtmlTag { get; set; }
        public bool IsClosed { get; set; }
        public string HtmlClosingTag => HtmlTag == "" ? "" : HtmlTag.Insert(1, "/");

        public Token(int startPos, int length, Token parent = null)
        {
            StartPosition = startPos;
            Length = length;
            SubTokens = new List<Token>();
            Parent = parent;
            Type = TokenType.Simple;
        }

        public void ReplaceOpeningTag(StringBuilder stringBuilder, ref int indexOffset)
        {
            if (Type != TokenType.Simple)
            {
                stringBuilder.Remove(StartPosition + indexOffset, OpeningTag.Length);
                stringBuilder.Insert(StartPosition + indexOffset, HtmlTag);
                indexOffset += HtmlTag.Length - OpeningTag.Length;
            }
        }

        public virtual void ReplaceClosingTag(StringBuilder stringBuilder, ref int indexOffset)
        {
            if (Type != TokenType.Simple)
            {
                stringBuilder.Remove(EndPosition + indexOffset - ClosingTag.Length, ClosingTag.Length);
                stringBuilder.Insert(EndPosition + indexOffset - ClosingTag.Length, HtmlClosingTag);
                indexOffset += HtmlClosingTag.Length - ClosingTag.Length;
            }
        }
    }

    class ItalicToken : Token
    {
        public ItalicToken(int startPos = 0, int length = 0, Token parent = null) : base(startPos, length, parent)
        {
            {
                Type = TokenType.Italic;
                OpeningTag = "_";
                ClosingTag = OpeningTag;
                HtmlTag = "<em>";
            }
        }
    }

    class StrongToken : Token
    {
        public StrongToken(int startPos = 0, int length = 0, Token parent = null) : base(startPos, length, parent)
        {
            {
                Type = TokenType.Strong;
                OpeningTag = "__";
                ClosingTag = OpeningTag;
                HtmlTag = "<strong>";
            }
        }
    }

    class HeaderToken : Token
    {
        public HeaderToken(int startPos = 0, int length = 0, Token parent = null) : base(startPos, length, parent)
        {
            {
                Type = TokenType.Header;
                OpeningTag = "# ";
                ClosingTag = "\n";
                HtmlTag = "<h1>";
            }
        }

        public override void ReplaceClosingTag(StringBuilder stringBuilder, ref int indexOffset)
        {
            if (stringBuilder[EndPosition + indexOffset - ClosingTag.Length] == '\n')
                stringBuilder.Insert(EndPosition + indexOffset - ClosingTag.Length, HtmlClosingTag);
            else
                stringBuilder.Insert(EndPosition + indexOffset, $"{HtmlClosingTag}\n");
            indexOffset += 5;
        }
    }
}