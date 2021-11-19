using System.Collections.Generic;

namespace Markdown
{
    public class TagItalic : ITag
    {
        public bool IsAtTheBeginning { get; set; }
        public bool IsClosed { get; set; }

        private string HtmlTagAnalog
        {
            get
            {
                if (IsClosed)
                    return (IsStartTag) ? "<em>" : "</em>";
                return "_";
            }
        }

        public bool IsStartTag { get; set; }

        public string Content => HtmlTagAnalog;

        public bool IsNotToPairToken { get; set; }

        public void GenerateProperties(LinkedListNode<IToken> currentToken)
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
                if (currentToken.Value is TagBold && currentToken.Value.IsNotToPairToken && boldCnt % 2 != 0)
                    break;
                if (currentToken.Value is TagItalic && !currentToken.Value.IsNotToPairToken)
                {
                    if (currentToken.Value is ITag starter)
                        if ((!starter.IsAtTheBeginning && spacesCnt == 0 || starter.IsAtTheBeginning && !IsAtTheBeginning) && !onlyEmptyStrings)
                            HtmlTokenAnalyzer.MakePair(starter, this);
                }

                if (!(currentToken.Value is TagWord && currentToken.Value.Content.Length == 0))
                    onlyEmptyStrings = false;
                currentToken = currentToken.Previous;
            }
        }
    }
}
