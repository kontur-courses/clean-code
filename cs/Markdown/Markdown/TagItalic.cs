using System.Collections.Generic;

namespace Markdown
{
    public class TagItalic : ITag
    {
        public bool IsAtTheBeginning { get; set; }
        public bool IsClosed { get; set; }

        public string HtmlTagAnalog => (IsStartTag) ? "<em>" : "</em>";

        public bool IsStartTag { get; set; }

        public string Content => "_";

        public bool IsNotToPairToken { get; set; }

        public void FindPairToken(LinkedListNode<IToken> currentToken)
        {
            var spacesCnt = 0;
            var boldCnt = 0;
            var onlyEmptyStrings = true;
            while (currentToken != null)
            {
                if (currentToken.Value is TagSpace)
                    spacesCnt++;
                if (currentToken.Value is TagBold)
                    boldCnt++;
                if (currentToken.Value is TagBold 
                    && currentToken.Value.IsNotToPairToken && boldCnt % 2 != 0)
                    break;
                if (currentToken.Value is TagItalic 
                    && !currentToken.Value.IsNotToPairToken)
                {
                    if (currentToken.Value is ITag starter)
                        if ((!starter.IsAtTheBeginning && spacesCnt == 0
                             || starter.IsAtTheBeginning && !IsAtTheBeginning) && !onlyEmptyStrings)
                            HtmlTokenAnalyzer.MakePair(starter, this);
                }

                if (!(currentToken.Value is TagWord
                      && currentToken.Value.Content.Length == 0))
                    onlyEmptyStrings = false;
                currentToken = currentToken.Previous;
            }
        }
    }
}
