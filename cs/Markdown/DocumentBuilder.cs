using System.Text;

namespace Markdown
{
    public class DocumentBuilder
    {
        private const char ParagraphSeparator = '\n';
        private const string ListTagName = "ul";

        private readonly StringBuilder html = new ();

        public void AddParagraph(string paragraphHtml, bool isListItem)
        {
            throw new NotImplementedException();
        }

        public string Build()
        {
            throw new NotImplementedException();
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
