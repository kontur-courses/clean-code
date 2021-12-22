namespace MarkdownTask
{
    public class Md
    {
        public string Render(string mdText)
        {
            var a = new SingleHighlightTagSearcher();
            var s = a.SearchForTags(mdText);
            return mdText;
        }
    }
}