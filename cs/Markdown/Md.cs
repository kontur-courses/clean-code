using System.Text;

namespace Markdown
{
    public class Md
    {
        private const char ParagraphSeparator = '\n';
        private const string ListTagName = "ul";

        private readonly ParagraphRenderer paragraphRenderer = new ();

        public string Render(string markdown)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<string> SplitIntoParagraphs(string markdown)
        {
            throw new NotImplementedException();
        }

        private static void OpenListIfStarted(StringBuilder html, bool wasListItem, bool isListItem)
        {
            throw new NotImplementedException();
        }

        private static void CloseListIfEnded(StringBuilder html, bool wasListItem, bool isListItem)
        {
            throw new NotImplementedException();
        }
    }
}
