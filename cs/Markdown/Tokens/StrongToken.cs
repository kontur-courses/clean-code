using System.Collections.Generic;

namespace Markdown
{
    class StrongToken : Token
    {
        public StrongToken(int startPos = 0, int length = 0, IToken parent = null) : base(startPos, length, parent)
        {
            {
                Type = TokenType.Strong;
                OpeningTag = "__";
                ClosingTag = OpeningTag;
                HtmlTag = "<strong>";
                NestingLevel = 3;
                ContainsOnlyDigits = true;
            }
        }

        public override void CloseToken(Stack<IToken> openedTokens, int position)
        {
            if (openedTokens.Peek().Type == TokenType.Italic && openedTokens.Peek().Parent.Type == TokenType.Strong)
                IToken.ClosePreviousOpenedToken(openedTokens, position);
            else if (openedTokens.Peek().Type == TokenType.Strong && openedTokens.Peek().Parent.Type != TokenType.Italic
                                                                  && !ContainsOnlyDigits)
                IToken.Close(openedTokens, position);
        }
    }
}
