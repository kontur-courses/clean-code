using System.Text;

namespace Markdown
{
    public class DocumentBuilder
    {
        private const char ParagraphSeparator = '\n';
        private const string ListTagName = "ul";

        private readonly StringBuilder html = new ();
        private bool hasParagraphs;

        public void AddParagraph(string paragraphHtml, bool isListItem)
        {
            if (hasParagraphs)
                html.Append(ParagraphSeparator);

            html.Append(paragraphHtml);
            hasParagraphs = true;
        }

        public string Build()
        {
            return html.ToString();
        }

        private void OpenListIfStarted(bool isListItem)
        {
            throw new NotImplementedException();
        }

        private void CloseListIfEnded(bool isListItem)
        {
            throw new NotImplementedException();
        }
    }
}
