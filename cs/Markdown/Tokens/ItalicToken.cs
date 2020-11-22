using System.Collections.Generic;

namespace Markdown
{
    class ItalicToken : Token
    {
        public ItalicToken(int startPos = 0, int length = 0, IToken parent = null) : base(startPos, length, parent)
        {
            {
                Type = TokenType.Italic;
                OpeningTag = "_";
                ClosingTag = OpeningTag;
                HtmlTag = "<em>";
                NestingLevel = 4;
                ContainsOnlyDigits = true;
            }
        }

        public override void CloseToken(Stack<IToken> openedTokens, int position)
        {
            if (!openedTokens.Peek().ContainsOnlyDigits)
                IToken.Close(openedTokens, position);
        }
    }
}