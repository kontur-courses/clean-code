using System.Collections.Generic;

namespace Markdown.Tags
{
    public class TagLink : ITag
    {
        public TagLink(string address)
        {
            Address = address;
        }

        public bool IsClosed { get; set; }

        public bool IsStartTag { get; set; }

        public bool IsAtTheBeginning { get; set; }

        public bool IsNotToPairTag { get; set; }

        public string HtmlTagAnalog => (IsStartTag) ? $"<a href=\"{Address}\">" : "</a>";

        public string Content => null;

        public string Address { get; set; }

        public void FindPairToken(LinkedListNode<IToken> currentToken)
        {
            while (currentToken != null)
            {
                if (currentToken.Value is TagLink link
                    && !link.IsNotToPairTag)
                { 
                    link.Address = Address; 
                    HtmlTokenAnalyzer.MakePair(link, this);
                }
                else if (currentToken.Value is ITag tagValue)
                    tagValue.IsClosed = false;
                currentToken = currentToken.Previous;
            }
        }
    }
}
