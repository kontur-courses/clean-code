using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public interface IToken
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
        public string HtmlClosingTag();

        public void OpenTag(StringBuilder newLine, string inputLine);

        public void CloseTag(StringBuilder newLine, string inputLine);
        public void CloseToken(Stack<IToken> openedTokens, int position);

        public static void Close(Stack<IToken> openedTokens, int startOfClosingTagPosition)
        {
            var closedToken = openedTokens.Pop();
            closedToken.IsClosed = true;
            closedToken.Length = startOfClosingTagPosition - closedToken.StartPosition + closedToken.ClosingTag.Length;
            if (closedToken.Length != closedToken.ClosingTag.Length + closedToken.OpeningTag.Length)
                openedTokens.Peek().SubTokens.Add(closedToken);
        }

        public static void ClosePreviousOpenedToken(Stack<IToken> openedTokens, int position)
        {
            var italicToken = openedTokens.Pop();
            Close(openedTokens, position);
            openedTokens.Push(italicToken);
        }

        public static void Open(IToken token, Stack<IToken> openedTokens)
        {
            token.Parent = openedTokens.Peek();
            openedTokens.Push(token);
        }
        public static void Remove(IToken token)
        {
            var parent = token.Parent;
            foreach (var closedSubToken in token.SubTokens.Where(t => t.IsClosed))
            {
                parent.SubTokens.Add(closedSubToken);
                closedSubToken.Parent = parent;
            }
            parent.SubTokens.Remove(token);
        }

        public static bool CheckIntersectionOfTokens(Stack<IToken> openedTokens)
        {
            if (openedTokens.Peek().Parent.IsClosed)
            {
                Remove(openedTokens.Peek().Parent);
                openedTokens.Pop();
                return true;
            }

            return false;
        }
    }
}