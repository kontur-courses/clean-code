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

        public void AddParagraph(string paragraphHtml)
        {
            CloseList();
            AppendSeparator();
            html.Append(paragraphHtml);
        }

        public void AddListItem(string listItemHtml)
        {
            AppendSeparator();
            OpenList();
            html.Append(listItemHtml);
        }

        public string Build()
        {
            CloseList();
            return html.ToString();
        }

        private void AppendSeparator()
        {
            if (hasParagraphs)
                html.Append(ParagraphSeparator);

            hasParagraphs = true;
        }

        private void OpenList()
        {
            if (isInsideList)
                return;

            html.Append(HtmlTag.Open(ListTagName));
            isInsideList = true;
        }

        private void CloseList()
        {
            if (!isInsideList)
                return;

            html.Append(HtmlTag.Close(ListTagName));
            isInsideList = false;
        }
    }
}
