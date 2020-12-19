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
        public Tag OpeningTag { get; set; }
        public Tag ClosingTag { get; set; }
        public bool ContainsOnlyDigits { get; set; }

        public void Open(Stack<IToken> openedTokens);

        public void Close(int position);
    }
}