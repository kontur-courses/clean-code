using System.Collections.Generic;

namespace Markdown
{
    class ReferenceToken : Token
    {
        public string Reference { get; set; }
        public ReferenceToken(int startPos, int length, string reference, IToken parent = null) : base(startPos, length, parent)
        {
            Reference = reference;
            Type = TokenType.Reference;
            OpeningTag = reference.Length == 0 ? "" : $"${reference}!";
            ClosingTag = "$";
            HtmlTag = $"<a href={reference}>";
            NestingLevel = 2;
        }

        public override void CloseToken(Stack<IToken> openedTokens, int position)
        {
            IToken.Close(openedTokens, position);
        }

        public override string HtmlClosingTag() => "</a>";
    }
}
