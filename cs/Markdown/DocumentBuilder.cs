using System.Text;

namespace Markdown
{
    public class DocumentBuilder
    {
        private const char ParagraphSeparator = '\n';
        private const string ListTagName = "ul";

        private readonly StringBuilder html = new ();
        private bool hasParagraphs;
        private bool isInsideList;

        public void AddParagraph(string paragraphHtml, bool isListItem)
        {
            if (hasParagraphs)
                html.Append(ParagraphSeparator);

            CloseListIfEnded(isListItem);
            OpenListIfStarted(isListItem);
            html.Append(paragraphHtml);
            hasParagraphs = true;
        }

        public string Build()
        {
            CloseListIfEnded(false);
            return html.ToString();
        }

        private void OpenListIfStarted(bool isListItem)
        {
            if (isListItem && !isInsideList)
            {
                html.Append(HtmlTag.Open(ListTagName));
                isInsideList = true;
            }
        }

        private void CloseListIfEnded(bool isListItem)
        {
            if (isInsideList && !isListItem)
            {
                html.Append(HtmlTag.Close(ListTagName));
                isInsideList = false;
            }
        }
    }
}
