using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public abstract class Token : IToken
    {
        public int StartPosition { get; set; }
        public int EndPosition => StartPosition + Length;
        public int Length { get; set; }
        public TokenType Type { get; set; }
        public List<IToken> SubTokens { get; set; }
        public IToken Parent { get; set; }
        public int NestingLevel { get; set; }
        public string OpeningTag { get; set; }
        public string ClosingTag { get; set; }
        public string HtmlTag { get; set; }
        public bool IsClosed { get; set; }
        public bool ContainsOnlyDigits { get; set; }
        public virtual string HtmlClosingTag() => HtmlTag == "" ? "" : HtmlTag.Insert(1, "/");

        public Token(int startPos, int length, IToken parent)
        {
            StartPosition = startPos;
            Length = length;
            SubTokens = new List<IToken>();
            Parent = parent;
        }

        public virtual void OpenTag(StringBuilder newLine, string inputLine)
        {
            if (Type != TokenType.Simple)
                newLine.Append(HtmlTag);
            newLine.Append(SubTokens.Any()
                ? inputLine.Substring(StartPosition + OpeningTag.Length,
                    SubTokens[0].StartPosition - OpeningTag.Length - StartPosition)
                : inputLine.Substring(StartPosition + OpeningTag.Length,
                    Length - OpeningTag.Length - ClosingTag.Length));
        }

        public virtual void CloseTag(StringBuilder newLine, string inputLine)
        {
            if (SubTokens.Any())
                newLine.Append(inputLine.Substring(SubTokens.Last().EndPosition,
                    EndPosition - ClosingTag.Length - SubTokens.Last().EndPosition));
            if (Type != TokenType.Simple)
                newLine.Append(HtmlClosingTag());
        }

        public virtual void CloseToken(Stack<IToken> openedTokens, int position)
        {
        }
    }
}