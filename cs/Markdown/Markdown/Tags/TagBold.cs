using System.Collections.Generic;

namespace Markdown.Tags
{
    public class TagBold : ITag
    {
        public bool IsAtTheBeginning { get; set; }

        public bool IsClosed { get; set; }

        public string HtmlTagAnalog => (IsStartTag) ? "<strong>" : "</strong>";

        public bool IsStartTag { get; set; }

        public string Content => "__";

        public bool IsNotToPairTag { get; set; }

        public void FindPairToken(LinkedListNode<IToken> currentToken)
        {
            var spaceCnt = 0;
            var onlyEmptyStrings = true;
            while (currentToken != null)
            {
                if (currentToken.Value is TokenSpace)
                    spaceCnt++;

                if ((currentToken.Value is ITag currentTag && currentTag.IsNotToPairTag))
                    break;

                if (currentToken.Value is TagItalic currentAsItalic 
                    && !currentAsItalic.IsClosed 
                    && !currentAsItalic.IsNotToPairTag)
                {
                    IsNotToPairTag = true;
                    break;
                }

                if (currentToken.Value is TagBold currentAsBold && !currentAsBold.IsNotToPairTag)
                {
                    if (currentToken.Value is ITag starter)
                        if ((!starter.IsAtTheBeginning && spaceCnt == 0
                             || starter.IsAtTheBeginning) && !onlyEmptyStrings)
                            HtmlTokenAnalyzer.MakePair(starter, this);
                }

                if (!(currentToken.Value is TokenWord && currentToken.Value.Content.Length == 0))
                    onlyEmptyStrings = false;
                currentToken = currentToken.Previous;
            }
        }
    }
}
