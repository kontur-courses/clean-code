using System.Collections.Generic;

namespace Markdown
{
    public class TagBold : ITag
    {
        public bool IsAtTheBeginning { get; set; }
        public bool IsClosed { get; set; }

        public string HtmlTagAnalog => (IsStartTag) ? "<strong>" : "</strong>";

        public bool IsStartTag { get; set; }

        public string Content => "__";

        public bool IsNotToPairToken { get; set; }

        public void FindPairToken(LinkedListNode<IToken> currentToken)
        {
            var spaceCnt = 0;
            var onlyEmptyStrings = true;
            while (currentToken != null)
            {
                if (currentToken.Value is TagSpace)
                    spaceCnt++;

                if (currentToken.Value.IsNotToPairToken)
                    break;

                if (currentToken.Value is TagItalic currentAsItalic 
                    && !currentAsItalic.IsClosed 
                    && !currentToken.Value.IsNotToPairToken)
                {
                    IsNotToPairToken = true;
                    break;
                }

                if (currentToken.Value is TagBold && !currentToken.Value.IsNotToPairToken)
                {
                    if (currentToken.Value is ITag starter)
                        if ((!starter.IsAtTheBeginning && spaceCnt == 0
                             || starter.IsAtTheBeginning) && !onlyEmptyStrings)
                            HtmlTokenAnalyzer.MakePair(starter, this);
                }

                if (!(currentToken.Value is TagWord && currentToken.Value.Content.Length == 0))
                    onlyEmptyStrings = false;
                currentToken = currentToken.Previous;
            }
        }
    }
}
