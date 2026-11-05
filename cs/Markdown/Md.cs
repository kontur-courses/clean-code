namespace Markdown
{
    public class Md
    {
        private static readonly string[] ParagraphSeparators = ["\r\n", "\n"];

        private readonly ParagraphRenderer paragraphRenderer = new ();

        public string Render(string markdown)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<string> SplitIntoParagraphs(string markdown)
        {
            throw new NotImplementedException();
        }
    }
}
