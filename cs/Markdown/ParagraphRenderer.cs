namespace Markdown
{
    public class ParagraphRenderer
    {
        private const string HeaderPrefix = "# ";
        private const string ListItemPrefix = "- ";
        private const string HeaderTagName = "h1";
        private const string ListItemTagName = "li";

        private readonly InlineRenderer inlineRenderer = new ();

        public string Render(string paragraph)
        {
            throw new NotImplementedException();
        }

        public bool IsListItem(string paragraph)
        {
            throw new NotImplementedException();
        }

        private static bool IsHeader(string paragraph)
        {
            throw new NotImplementedException();
        }

        private string RenderHeader(string paragraph)
        {
            throw new NotImplementedException();
        }

        private string RenderListItem(string paragraph)
        {
            throw new NotImplementedException();
        }

        private static string RemovePrefix(string paragraph, string prefix)
        {
            throw new NotImplementedException();
        }
    }
}
